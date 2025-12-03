# АС-Configurator СЗІ (Enterprise Edition)

Повноцінна Windows-утиліта WPF (.NET 6) для керування політиками безпеки за профілями АС-1/АС-2/АС-3 та вимогами НД ТЗІ 2.5-008-2002.

## Можливості
- Профілі безпеки у JSON, організаційні та технічні вимоги.
- Training/Audit/Apply режими (portable режим дозволяє лише Training/Audit).
- Risk Matrix з підрахунком Critical/High/Medium/Low.
- Compliance Score для АС-1/АС-2/АС-3.
- Виявлення GPO-контролю та блокування редагування.
- Генерація паспортів безпеки у DOCX/PDF.
- Self-Healing з періодичною перевіркою політик.
- Виявлення небезпечного ПЗ (TeamViewer/AnyDesk/Radmin/майнери).
- Резервні копії secedit /export перед застосуванням політик.
- Плагінна архітектура для розширення.

## Структура
```
/UI
  /Views
  /ViewModels
/Core
  /Models
  /Services
  /Policies
  /Profiles
  /Compliance
  /RiskAnalysis
/Infrastructure
  /Logging
  /INI
  /PowerShell
  /Security
  /GPO
/Reports
  /Docx
  /Pdf
/Plugins
/Config
/Tests
```

## Збірка
1. Встановіть .NET 6 SDK + Windows Desktop Runtime.
2. `dotnet restore ASConfigurator.sln`
3. `dotnet build ASConfigurator.sln` (усуває помилку MSB1011 про кілька проектів у каталозі)
4. Запуск: `dotnet run --project ASConfigurator.csproj`

### Збірка інсталяційного exe
Для отримання самостійного виконуваного файлу під Windows використовуйте публікацію:

```
dotnet publish ASConfigurator.csproj -c Release -r win10-x64 -p:PublishSingleFile=true -p:IncludeAllContentForSelfExtract=true --self-contained false -o publish
```

Після завершення у каталозі `publish/` буде доступний `ASConfigurator.exe`, який можна копіювати та запускати на цільовій системі з установленим .NET 6 Desktop Runtime. Для повністю самодостатньої збірки додайте `--self-contained true` (збільшить розмір, але не потребує попередньо встановленого рантайму).

## Тести
```
dotnet test ASConfigurator.Tests.csproj
```

## Portable режим
Створіть файл `portable.mode` поруч із виконуваним файлом. Дані та звіти зберігаються локально, застосування політик відключено.

## Приклади
- Профілі: `Config/Profiles/*.json`
- Зразок звітів: `Reports/Docx/SamplePassport.docx`, `Reports/Pdf/SamplePassport.pdf`
- Risk-matrix таблиця: `Reports/Pdf/risk-matrix-example.md`

## CI/CD
Додано GitHub Actions workflow `.github/workflows/ci.yml` для збірки, тестів та публікації артефактів.
