using System;
using Code.PanelManager;
using Code.PanelManager.Attributes;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Code.Core.Interactive
{
	[Panel(PanelType = PanelType.OVERLAY, Order = 2, AssetId = "InfoObject/Prefabs/InfoObjectPanel.prefab")]
	public class InfoObjectPanelController : PanelControllerBase<InfoObjectPanel>
	{
		private const int TIME = 1000;
		private const int TIME_PAUSE = 700;

		private CancellationTokenSource _cancellation_token_source;
		
		public async UniTask StartInfo(InfoObject.Info info)
		{
			if (_cancellation_token_source != null)
			{
				_cancellation_token_source.Cancel();
				_cancellation_token_source.Dispose();
			}
			
			_cancellation_token_source = new CancellationTokenSource();
			
			Open();
			
			for (int index = 0, count = info.sequence.Count; index < count; ++index)
			{
				panel.info_tmp.text = "";
				
				var text = info.sequence[index];

				var duration = TIME;
				
				if (text.Length <= 10)
				{
					duration /= 2;
				}
				
				var time =  duration / text.Length;
				
				foreach (var c in text)
				{
					panel.info_tmp.text += c;

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
				
			}
			
			Close();

			_cancellation_token_source.Dispose();
			_cancellation_token_source = null;
		}
	}
}