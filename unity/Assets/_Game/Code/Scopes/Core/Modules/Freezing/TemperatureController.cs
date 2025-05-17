using System.Collections.Generic;
using System.Linq;
using _Game.Code.Scopes.Shared;
using _Game.Code.Scopes.Shared.Interfaces;
using JetBrains.Annotations;
using UnityEngine;

namespace Code.Core.Modules.Freezing
{
	[UsedImplicitly]
	public class TemperatureController : IUpdatable
	{
		private readonly List<ITemperatureAffected> _targets = new();
		private readonly List<ITemperatureHeatSource> _heat_sources = new();

		public TemperatureController(MonoHeart heart)
		{
			heart.RegisterUpdatable(this);
		}
		
		public void RegisterColdTarget(ITemperatureAffected target)
		{
			if (_targets.Contains(target))
			{
				return;
			}

			_targets.Add(target);
		}

		public void UnregisterColdTarget(ITemperatureAffected target)
		{
			_targets.Remove(target);
		}

		public void RegisterHeatSource(ITemperatureHeatSource source)
		{
			if (_heat_sources.Contains(source))
			{
				return;
			}

			_heat_sources.Add(source);
		}

		public void UnregisterHeatSource(ITemperatureHeatSource source)
		{
			_heat_sources.Remove(source);
		}

		public void Update(float delta_time)
		{
			const float FREEZE_STRENGTH_MULTIPLIER = 0.1f;
			const float THAW_MULTIPLIER = 0.1f;
			const float THAW_THRESHOLD = 10f;

			var global_temperature = CalculateGlobalTemperature();

			for (int index_1 = 0, count_1 = _targets.Count; index_1 < count_1; ++index_1)
			{
				var target = _targets[index_1];
				var heat = _heat_sources.Sum(source => source.GetHeatAt(target)); // 0..100
				var parts = target.Body.parts;

				for (int index_2 = 0, count_2 = parts.Count; index_2 < count_2; ++index_2)
				{
					var part = parts[index_2];
					var resistance = part.CalculateResistance01(); // 0..1

					// Текущая температура замерзания (0 = лёд, 100 = норма)
					float current_value = part.value;

					// Эффективная температура окружения с учетом тепла и сопротивления
					float effective_temperature = global_temperature * (1f - resistance) + heat;

					// Расчёт изменения температуры части тела
					float delta = 0f;

					if (effective_temperature < 0f)
					{
						float freeze_strength = -effective_temperature * (1f - resistance);
						delta = -freeze_strength * delta_time * FREEZE_STRENGTH_MULTIPLIER;
					}
					else if (effective_temperature >= THAW_THRESHOLD)
					{
						float thaw_strength = (effective_temperature - THAW_THRESHOLD) * (1f + resistance);
						delta = thaw_strength * delta_time * THAW_MULTIPLIER;
					}

					// Вычисляем новую температуру и применяем
					float new_value = Mathf.Clamp(current_value + delta, 0f, 100f);
					part.Apply(new_value);
				}
			}
		}



		private float CalculateGlobalTemperature()
		{
			return -10;
		}
		
		// Дельта — стремимся к равновесию между теплом/холодом и текущей температурой
		//var delta = (heat + target_temperature - part.value) * TEMPERATURE_MULTIPLIER;
		//var new_temp = Mathf.Clamp(part.value + delta * delta_time, 0f, 100f);
		//part.Apply(new_temp);
	}
}