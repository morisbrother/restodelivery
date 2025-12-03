using System.Collections.Generic;
using ASConfigurator.Core.Models;

namespace ASConfigurator.Core.Services
{
    public class TrainingExplanationService
    {
        public string BuildExplanation(PolicySetting setting)
        {
            var lines = new List<string>
            {
                $"Параметр: {setting.Name}",
                "Важливість: забезпечує відповідність НД ТЗІ 2.5-008-2002.",
                $"Ризик: {setting.Risk}",
                "Рекомендації: застосувати політику через локальні засоби або GPO.",
                "Приклад атаки: неправильне значення дозволяє обхід контролю доступу.",
                $"Профіль: {setting.ProfileLevel}"
            };
            return string.Join("\n", lines);
        }
    }
}
