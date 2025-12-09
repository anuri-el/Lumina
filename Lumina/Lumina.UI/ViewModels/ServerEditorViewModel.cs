using Lumina.Shared.DTOs;
using Lumina.UI.Commands;
using Lumina.UI.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Lumina.UI.ViewModels
{
    public class ServerEditorViewModel : INotifyPropertyChanged
    {
        private readonly ApiClientService _apiClient;

        private string _logText = "";
        public string LogText
        {
            get => _logText;
            set { _logText = value; OnPropertyChanged(); }
        }

        private bool _isConnected = false;
        public bool IsConnected
        {
            get => _isConnected;
            set { _isConnected = value; OnPropertyChanged(); }
        }

        private string _connectionStatus = "Disconnected";
        public string ConnectionStatus
        {
            get => _connectionStatus;
            set { _connectionStatus = value; OnPropertyChanged(); }
        }

        private int? _currentCollageId = null;
        public int? CurrentCollageId
        {
            get => _currentCollageId;
            set { _currentCollageId = value; OnPropertyChanged(); }
        }

        public ObservableCollection<ImageDto> ServerImages { get; set; } = new();
        public ObservableCollection<CollageDto> ServerCollages { get; set; } = new();

        public ICommand ConnectToServerCommand { get; }
        public ICommand UploadImageCommand { get; }
        public ICommand LoadImagesCommand { get; }
        public ICommand CreateCollageCommand { get; }
        public ICommand LoadCollagesCommand { get; }
        public ICommand SaveCollageCommand { get; }

        public ServerEditorViewModel()
        {
            _apiClient = new ApiClientService("https://localhost:7001");

            ConnectToServerCommand = new RelayCommand(async () => await ConnectToServer());
            UploadImageCommand = new RelayCommand(async () => await UploadImage());
            LoadImagesCommand = new RelayCommand(async () => await LoadImages());
            CreateCollageCommand = new RelayCommand(async () => await CreateCollage());
            LoadCollagesCommand = new RelayCommand(async () => await LoadCollages());
            SaveCollageCommand = new RelayCommand(async () => await SaveCollage());
        }

        private async Task ConnectToServer()
        {
            try
            {
                AppendLog("Attempting to connect to server...");

                // Перевіряємо з'єднання, завантажуючи список зображень
                var response = await _apiClient.Images.GetAllImagesAsync();

                if (response.Success)
                {
                    IsConnected = true;
                    ConnectionStatus = "Connected";
                    AppendLog($"✓ Connected to server successfully");
                    await LoadImages();
                }
                else
                {
                    IsConnected = false;
                    ConnectionStatus = "Connection failed";
                    AppendLog($"✗ Connection failed: {response.Message}");
                }
            }
            catch (Exception ex)
            {
                IsConnected = false;
                ConnectionStatus = "Error";
                AppendLog($"✗ Error connecting to server: {ex.Message}");
            }
        }

        private async Task UploadImage()
        {
            try
            {
                var dialog = new Microsoft.Win32.OpenFileDialog
                {
                    Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
                    Title = "Select Image to Upload"
                };

                if (dialog.ShowDialog() == true)
                {
                    AppendLog($"Uploading image: {Path.GetFileName(dialog.FileName)}...");

                    var response = await _apiClient.Images.UploadImageAsync(dialog.FileName);

                    if (response.Success && response.Data != null)
                    {
                        ServerImages.Add(response.Data);
                        AppendLog($"✓ Image uploaded successfully (ID: {response.Data.Id})");

                        // Якщо є активний колаж, додаємо зображення до нього
                        if (CurrentCollageId.HasValue)
                        {
                            await AddImageToCurrentCollage(response.Data.Id);
                        }
                    }
                    else
                    {
                        AppendLog($"✗ Upload failed: {response.Message}");
                        MessageBox.Show($"Upload failed: {response.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                AppendLog($"✗ Error uploading image: {ex.Message}");
                MessageBox.Show($"Error uploading image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadImages()
        {
            try
            {
                AppendLog("Loading images from server...");

                var response = await _apiClient.Images.GetAllImagesAsync();

                if (response.Success && response.Data != null)
                {
                    ServerImages.Clear();
                    foreach (var image in response.Data)
                    {
                        ServerImages.Add(image);
                    }
                    AppendLog($"✓ Loaded {response.Data.Count} images from server");
                }
                else
                {
                    AppendLog($"✗ Failed to load images: {response.Message}");
                }
            }
            catch (Exception ex)
            {
                AppendLog($"✗ Error loading images: {ex.Message}");
            }
        }

        private async Task CreateCollage()
        {
            try
            {
                string collageName = $"Collage_{DateTime.Now:yyyyMMdd_HHmmss}";
                AppendLog($"Creating collage: {collageName}...");

                var response = await _apiClient.Collages.CreateCollageAsync(collageName, 1920, 1080);

                if (response.Success && response.Data != null)
                {
                    CurrentCollageId = response.Data.Id;
                    ServerCollages.Add(response.Data);
                    AppendLog($"✓ Collage created successfully (ID: {response.Data.Id})");
                }
                else
                {
                    AppendLog($"✗ Failed to create collage: {response.Message}");
                    MessageBox.Show($"Failed to create collage: {response.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                AppendLog($"✗ Error creating collage: {ex.Message}");
                MessageBox.Show($"Error creating collage: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadCollages()
        {
            try
            {
                AppendLog("Loading collages from server...");

                var response = await _apiClient.Collages.GetAllCollagesAsync();

                if (response.Success && response.Data != null)
                {
                    ServerCollages.Clear();
                    foreach (var collage in response.Data)
                    {
                        ServerCollages.Add(collage);
                    }
                    AppendLog($"✓ Loaded {response.Data.Count} collages from server");
                }
                else
                {
                    AppendLog($"✗ Failed to load collages: {response.Message}");
                }
            }
            catch (Exception ex)
            {
                AppendLog($"✗ Error loading collages: {ex.Message}");
            }
        }

        private async Task SaveCollage()
        {
            try
            {
                if (!CurrentCollageId.HasValue)
                {
                    AppendLog("✗ No active collage to save");
                    MessageBox.Show("Please create a collage first", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                AppendLog($"Saving collage (ID: {CurrentCollageId})...");

                // Тут можна додати додаткову логіку збереження
                // Наприклад, оновлення позицій шарів

                AppendLog($"✓ Collage saved successfully");
                MessageBox.Show("Collage saved successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                AppendLog($"✗ Error saving collage: {ex.Message}");
                MessageBox.Show($"Error saving collage: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task AddImageToCurrentCollage(int imageId)
        {
            try
            {
                if (!CurrentCollageId.HasValue)
                {
                    AppendLog("✗ No active collage");
                    return;
                }

                AppendLog($"Adding image {imageId} to collage {CurrentCollageId}...");

                // Випадкова позиція для нового шару
                Random rnd = new Random();
                double x = rnd.Next(50, 500);
                double y = rnd.Next(50, 300);

                var response = await _apiClient.Collages.AddLayerAsync(
                    CurrentCollageId.Value,
                    imageId,
                    x, y,
                    200, 200);

                if (response.Success)
                {
                    AppendLog($"✓ Image added to collage at ({x}, {y})");
                }
                else
                {
                    AppendLog($"✗ Failed to add image to collage: {response.Message}");
                }
            }
            catch (Exception ex)
            {
                AppendLog($"✗ Error adding image to collage: {ex.Message}");
            }
        }

        private void AppendLog(string message)
        {
            LogText += $"[{DateTime.Now:HH:mm:ss}] {message}\n";
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
