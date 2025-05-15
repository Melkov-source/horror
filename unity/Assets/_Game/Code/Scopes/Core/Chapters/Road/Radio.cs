using UnityEngine;

namespace Code.Core.Chapters.Road
{
	public class Radio : MonoBehaviour
	{
		public bool Enable
		{
			get => _enabled;
			set => SetEnabled(value);
		}

		[SerializeField] private AudioSource _audio_source;
		[SerializeField] private AudioClip[] _musics;

		private bool _enabled;
		private int _current_track_index;
		private bool _is_playing;
		
		private void Update()
		{
			if (_enabled == false)
			{
				return;
			}
			
			if (_audio_source.isPlaying == false && _is_playing)
			{
				NextMusic();
			}
		}
		
		public void NextMusic()
		{
			if (_musics.Length == 0)
			{
				return;
			}

			_current_track_index = (_current_track_index + 1) % _musics.Length;
			
			if (_enabled)
			{
				PlayMusic();
			}
		}

		public void BackMusic()
		{
			if (_musics.Length == 0)
			{
				return;
			}

			_current_track_index = (_current_track_index - 1 + _musics.Length) % _musics.Length;
			
			if (_enabled)
			{
				PlayMusic();
			}
		}

		private void SetEnabled(bool value)
		{
			_enabled = value;

			if (_enabled)
			{
				PlayMusic();
			}
			else
			{
				StopMusic();
			}
		}

		private void PlayMusic()
		{
			if (_enabled == false || _musics.Length == 0)
			{
				return;
			}
				

			_audio_source.clip = _musics[_current_track_index];
			_audio_source.Play();
			_is_playing = true;
		}

		private void StopMusic()
		{
			_audio_source.Stop();
			_is_playing = false;
		}
	}
}