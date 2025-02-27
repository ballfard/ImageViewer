using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using ReactiveUI;

namespace ImageViewer.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private Bitmap? _currentImage;
        private int _currentIndex;
        private ObservableCollection<string> _imagePaths;
        
        public ReactiveCommand<Unit, Unit> OpenFolderCommand { get; }
        public ReactiveCommand<Unit, Unit> NextImageCommand { get; }
        public ReactiveCommand<Unit, Unit> PreviousImageCommand { get; }
        
        public MainWindowViewModel()
        {
            _imagePaths = new ObservableCollection<string>();
            
            // Use the UI thread scheduler for commands
            var uiScheduler = RxApp.MainThreadScheduler;
            
           OpenFolderCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                try
                {
                    var mainWindow = (App.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
                    if (mainWindow != null)
                    {
                        // Configure folder picker with no restrictions
                        var folderPickerOptions = new FolderPickerOpenOptions
                        {
                            Title = "Select Image Folder",
                            AllowMultiple = false
                        };

                        Console.WriteLine("Opening folder picker dialog...");
                        var result = await mainWindow.StorageProvider.OpenFolderPickerAsync(folderPickerOptions);
                        
                        if (result != null && result.Count > 0)
                        {
                            var folderPath = result[0].Path.LocalPath;
                            Console.WriteLine($"Selected folder: {folderPath}");
                            
                            // Use Dispatcher to update the UI on the main thread
                            await Dispatcher.UIThread.InvokeAsync(() => 
                            {
                                LoadImagesFromFolder(folderPath);
                            });
                        }
                        else
                        {
                            Console.WriteLine("No folder selected");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error opening folder: {ex.Message}");
                    Console.WriteLine($"Stack trace: {ex.StackTrace}");
                }
            }, outputScheduler: RxApp.MainThreadScheduler);
                        
                        NextImageCommand = ReactiveCommand.Create(() => 
                        {
                            if (_imagePaths.Count > 0)
                            {
                                _currentIndex = (_currentIndex + 1) % _imagePaths.Count;
                                LoadImage(_imagePaths[_currentIndex]);
                            }
                        }, outputScheduler: uiScheduler);
                        
                        PreviousImageCommand = ReactiveCommand.Create(() => 
                        {
                            if (_imagePaths.Count > 0)
                            {
                                _currentIndex = (_currentIndex - 1 + _imagePaths.Count) % _imagePaths.Count;
                                LoadImage(_imagePaths[_currentIndex]);
                            }
                        }, outputScheduler: uiScheduler);
                    }
                    
        private void LoadImagesFromFolder(string folderPath)
        {
            try
            {
                _imagePaths.Clear();
                
                if (string.IsNullOrEmpty(folderPath) || !Directory.Exists(folderPath))
                {
                    return;
                }
                
                var imageExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };
                var files = Directory.GetFiles(folderPath)
                    .Where(file => imageExtensions.Contains(Path.GetExtension(file).ToLowerInvariant()))
                    .ToList();
                
                foreach (var file in files)
                {
                    _imagePaths.Add(file);
                }
                
                if (_imagePaths.Count > 0)
                {
                    _currentIndex = 0;
                    LoadImage(_imagePaths[0]);
                }
                else
                {
                    // Clear the current image if no images were found
                    Dispatcher.UIThread.Post(() => 
                    {
                        CurrentImage?.Dispose();
                        CurrentImage = null;
                        this.RaisePropertyChanged(nameof(ImageName));
                        this.RaisePropertyChanged(nameof(ImageInfo));
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading images: {ex.Message}");
            }
        }
        
        private void LoadImage(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    // Use Dispatcher to ensure we're on the UI thread
                    Dispatcher.UIThread.Post(() => 
                    {
                        // Dispose the old image to prevent memory leaks
                        if (CurrentImage != null)
                        {
                            var oldImage = CurrentImage;
                            CurrentImage = null;
                            oldImage.Dispose();
                        }
                        
                        // Load the new image
                        CurrentImage = new Bitmap(path);
                        this.RaisePropertyChanged(nameof(CurrentImage));
                        this.RaisePropertyChanged(nameof(ImageName));
                        this.RaisePropertyChanged(nameof(ImageInfo));
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading image {path}: {ex.Message}");
                Dispatcher.UIThread.Post(() => 
                {
                    CurrentImage = null;
                    this.RaisePropertyChanged(nameof(CurrentImage));
                });
            }
        }
        
        public Bitmap? CurrentImage
        {
            get => _currentImage;
            private set => this.RaiseAndSetIfChanged(ref _currentImage, value);
        }
        
        public string ImageName => _imagePaths.Count > 0 ? 
            Path.GetFileName(_imagePaths[_currentIndex]) : "No image loaded";
            
        public string ImageInfo => _imagePaths.Count > 0 ? 
            $"Image {_currentIndex + 1} of {_imagePaths.Count}" : "";
    }
}