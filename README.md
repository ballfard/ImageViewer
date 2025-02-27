# Avalonia Image Viewer

A simple cross-platform image viewer built with Avalonia UI that allows you to browse through images in a folder.

## Features

- Browse through images in a selected folder
- Support for common image formats (JPG, PNG, GIF, BMP)
- Navigation controls to move between images
- Display of current image information
- Cross-platform compatibility (Windows, macOS, Linux)

## Requirements

- .NET SDK 7.0 or later
- IDE of your choice (Visual Studio, VS Code, Rider, etc.)

## Getting Started

### Installation

1. Clone this repository or download the source code
2. Open a terminal in the project directory
3. Run the application:

```bash
dotnet run
```

### Usage

1. Launch the application
2. Click the "Open Folder" button to select a directory containing images
3. Navigate through the images using the "Previous" and "Next" buttons
4. The bottom status bar shows the current image name and position in the collection

## Project Structure

- `App.axaml` & `App.axaml.cs`: Application entry point and initialization
- `MainWindow.axaml`: UI layout definition
- `MainWindow.axaml.cs`: UI code-behind
- `ViewModels/MainWindowViewModel.cs`: Contains the application logic
- `Program.cs`: .NET entry point and Avalonia configuration

## Technologies Used

- [Avalonia UI](https://avaloniaui.net/): Cross-platform .NET UI framework
- [ReactiveUI](https://reactiveui.net/): MVVM framework for building reactive applications
- .NET 7.0+: Modern C# development platform

## Learning Points

This project demonstrates several key concepts:

- Cross-platform desktop UI development
- MVVM architecture pattern
- File system operations
- Image loading and display
- Reactive programming with ReactiveUI
- Thread-safe UI updates

## Future Enhancements

Potential improvements for the future:

- Add image zooming and rotation capabilities
- Implement slideshow functionality
- Add basic image editing features
- Display image metadata (EXIF information)
- Add thumbnail view for easier navigation
- Support for additional image formats

## Troubleshooting

- If images aren't loading, check if they're in a supported format (JPG, PNG, GIF, BMP)
- Make sure the application has read permissions for the selected folder
- For very large images, the application might need more memory
- Check the console output for any error messages

## License

This project is open source and available under the MIT License.

---

Built with Avalonia UI as a learning project.
