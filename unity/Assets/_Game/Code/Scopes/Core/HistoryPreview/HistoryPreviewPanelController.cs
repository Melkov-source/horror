using System;
using System.Threading;
using Code.Core.Interactive;
using Code.PanelManager;
using Code.PanelManager.Attributes;
using Cysharp.Threading.Tasks;

namespace Code.Core.HistoryPreview
{
	[Panel(PanelType = PanelType.OVERLAY, Order = 4, AssetId = "HistoryPreview/Prefabs/HistoryPreview.prefab")]
	public class HistoryPreviewPanelController : PanelControllerBase<HistoryPreviewPanel>
	{
		private const int TIME = 1000;
		private const int TIME_PAUSE = 700;

		private CancellationTokenSource _cancellation_token_source;
		
		public async UniTask StartText(string text)
		{
			if (_cancellation_token_source != null)
			{
				_cancellation_token_source.Cancel();
				_cancellation_token_source.Dispose();
			}
			
			_cancellation_token_source = new CancellationTokenSource();
			
			Open();

			var sequence = text.Split('\n');
			
			panel.tmp.text = "";
			
			for (int index = 0, count = sequence.Length; index < count; ++index)
			{
				var line = sequence[index];

				var duration = TIME;
				
				if (line.Length <= 10)
				{
					duration /= 2;
				}
				
				var time =  duration / line.Length;
				
				foreach (var c in line)
				{
					panel.tmp.text += c;

					try
					{
						await UniTask.Delay(time, cancellationToken: _cancellation_token_source.Token);
					}
					catch (Exception e)
					{
						return;
					}
				}

				try
				{
					await UniTask.Delay(TIME_PAUSE, cancellationToken: _cancellation_token_source.Token);
				}
				catch (Exception e)
				{
					return;
				}

				if (index != sequence.Length - 1)
				{
					panel.tmp.text += '\n';
				}
			}
			
			Close();

			_cancellation_token_source.Dispose();
			_cancellation_token_source = null;
		}
	}
}