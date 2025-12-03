using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using ASConfigurator.Core.Compliance;
using ASConfigurator.Core.Models;
using ASConfigurator.Core.RiskAnalysis;
using ASConfigurator.Core.Services;
using ASConfigurator.Infrastructure.GPO;
using ASConfigurator.Infrastructure.Logging;
using ASConfigurator.Infrastructure.Security;

namespace ASConfigurator.UI.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly ProfileService _profileService;
        private readonly PolicyService _policyService;
        private readonly RiskAnalyzer _riskAnalyzer;
        private readonly ComplianceService _complianceService;
        private readonly TrainingExplanationService _trainingExplanationService;
        private readonly PortableService _portableService;
        private readonly GpoService _gpoService;
        private readonly ReportService _reportService;
        private readonly BackupService _backupService;
        private readonly SecurityScanner _securityScanner;
        private readonly LogService _logService;
        private readonly SelfHealingService _selfHealingService;

        private string _selectedMode = "Training";
        private PolicySetting? _selectedPolicy;
        private string _explanation = string.Empty;
        private ObservableCollection<SecurityProfile> _profiles = new();
        private ObservableCollection<PolicySetting> _displayedPolicies = new();
        private SecurityProfile? _selectedProfile;
        private RiskSummary _riskSummary = new();
        private ComplianceScores _complianceScores = new();

        public MainWindowViewModel()
        {
            _profileService = new ProfileService();
            _policyService = new PolicyService();
            _riskAnalyzer = new RiskAnalyzer();
            _complianceService = new ComplianceService();
            _trainingExplanationService = new TrainingExplanationService();
            _portableService = new PortableService();
            _gpoService = new GpoService();
            _reportService = new ReportService();
            _backupService = new BackupService();
            _securityScanner = new SecurityScanner();
            _logService = new LogService();
            _selfHealingService = new SelfHealingService(_logService);

            RefreshCommand = new RelayCommand(async () => await RefreshAsync());
            AnalyzeRiskCommand = new RelayCommand(() => AnalyzeRisk());
            ExplainPolicyCommand = new RelayCommand(() => ExplainSelected());
            GeneratePassportCommand = new RelayCommand(async () => await GeneratePassportAsync());
            SelfHealingCommand = new RelayCommand(StartSelfHealing);
            AvailableModes = new ObservableCollection<string>(new[] { "Training", "Audit", "Apply" });

            _ = RefreshAsync();
        }

        public ObservableCollection<string> AvailableModes { get; }
        public ObservableCollection<SecurityProfile> Profiles { get => _profiles; set { _profiles = value; OnPropertyChanged(); } }
        public ObservableCollection<PolicySetting> DisplayedPolicies { get => _displayedPolicies; set { _displayedPolicies = value; OnPropertyChanged(); } }
        public ObservableCollection<string> DangerousSoftware => new(_securityScanner.DetectDangerousSoftware());
        public SecurityProfile? SelectedProfile
        {
            get => _selectedProfile;
            set { _selectedProfile = value; OnPropertyChanged(); UpdateDisplayedPolicies(); }
        }
        public PolicySetting? SelectedPolicy { get => _selectedPolicy; set { _selectedPolicy = value; OnPropertyChanged(); } }
        public RiskSummary RiskSummary { get => _riskSummary; set { _riskSummary = value; OnPropertyChanged(); } }
        public ComplianceScores ComplianceScores { get => _complianceScores; set { _complianceScores = value; OnPropertyChanged(); } }
        public string SelectedMode { get => _selectedMode; set { _selectedMode = value; OnPropertyChanged(); } }
        public bool IsPortable => _portableService.IsPortable();
        public string Explanation { get => _explanation; set { _explanation = value; OnPropertyChanged(); } }

        public ICommand RefreshCommand { get; }
        public ICommand AnalyzeRiskCommand { get; }
        public ICommand ExplainPolicyCommand { get; }
        public ICommand GeneratePassportCommand { get; }
        public ICommand SelfHealingCommand { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        private async Task RefreshAsync()
        {
            var profiles = _profileService.LoadProfiles();
            var runtimeStatus = new Dictionary<string, string>
            {
                {"MinPasswordLength", "Виконано"},
                {"Firewall Domain", "Виконано"},
                {"RDP", "Не застосовано"}
            };

            foreach (var profile in profiles)
            {
                _policyService.MergeWithRuntime(profile.Policies, runtimeStatus);
                ApplyGpoInfo(profile);
            }

            Profiles = new ObservableCollection<SecurityProfile>(profiles);
            SelectedProfile = Profiles.FirstOrDefault();
            AnalyzeRisk();
            ComplianceScores = _complianceService.Calculate(profiles);
            await _logService.FlushAsync();
        }

        private void ApplyGpoInfo(SecurityProfile profile)
        {
            var gpoControlled = _gpoService.DetectControlledPolicies();
            foreach (var policy in profile.Policies)
            {
                if (gpoControlled.TryGetValue(policy.Name, out var gpo))
                {
                    policy.IsGpoControlled = true;
                    policy.GpoName = gpo;
                }
            }
        }

        private void UpdateDisplayedPolicies()
        {
            DisplayedPolicies = new ObservableCollection<PolicySetting>(SelectedProfile?.Policies ?? new List<PolicySetting>());
        }

        private void AnalyzeRisk()
        {
            if (SelectedProfile == null)
            {
                return;
            }

            RiskSummary = _riskAnalyzer.Analyze(SelectedProfile.Policies);
        }

        private void ExplainSelected()
        {
            if (SelectedPolicy == null)
            {
                Explanation = string.Empty;
                return;
            }

            Explanation = _trainingExplanationService.BuildExplanation(SelectedPolicy);
        }

        private async Task GeneratePassportAsync()
        {
            if (SelectedProfile == null)
            {
                return;
            }

            await _backupService.CreateBackupAsync();
            await _reportService.GenerateSecurityPassportAsync(Environment.MachineName, SelectedProfile, RiskSummary, ComplianceScores);
        }

        private void StartSelfHealing()
        {
            _selfHealingService.Start(TimeSpan.FromHours(4));
        }

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
