using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using WindowsGame.Data;
using WindowsGame.Static;
using MediaPlayerX = Microsoft.Xna.Framework.Media.MediaPlayer;

namespace WindowsGame.Managers
{
	public interface ISoundManager
	{
		void PlayCandySoundEffect();
		void PlayBonusSoundEffect();
		void PlayDeathSoundEffect();
		void PlayKilldSoundEffect();
		void PlayMetalSoundEffect();
		void PlayExtraSoundEffect();
		void PlayHappySoundEffect();
		void PlayOverXSoundEffect();
		void PlayReadySoundEffect();
		void PlayTrialBuzzdEffect();

		void StartMusic();
		void StopMusic();
		void PauseMusic();
		void ResumeMusic();

		void StartWinner();
		void StopWinner();
	}

	public class SoundManager : ISoundManager
	{
		public void PlayCandySoundEffect()
		{
			PlaySoundEffect(SoundEffectType.EatCandy);
		}
		public void PlayBonusSoundEffect()
		{
			PlaySoundEffect(SoundEffectType.EatBonus);
		}
		public void PlayDeathSoundEffect()
		{
			PlaySoundEffect(SoundEffectType.GamerDie1);
		}
		public void PlayKilldSoundEffect()
		{
			PlaySoundEffect(SoundEffectType.GamerKill);
		}
		public void PlayMetalSoundEffect()
		{
			PlaySoundEffect(SoundEffectType.MetalSolo);
		}
		public void PlayExtraSoundEffect()
		{
			PlaySoundEffect(SoundEffectType.GetExtra);
		}
		public void PlayHappySoundEffect()
		{
			PlaySoundEffect(SoundEffectType.Celebrate);
		}
		public void PlayOverXSoundEffect()
		{
			PlaySoundEffect(SoundEffectType.GameOver);
		}
		public void PlayReadySoundEffect()
		{
			PlaySoundEffect(SoundEffectType.GetReady);
		}
		public void PlayTrialBuzzdEffect()
		{
			PlaySoundEffect(SoundEffectType.TrialBuzz);
		}

		public void StartMusic()
		{
			if (!BaseData.UsePlayMusic)
			{
				return;
			}

			if (MediaState.Playing == MediaPlayerX.State)
			{
				return;
			}

			MediaPlayerX.Play(Assets.Song);
			MediaPlayerX.IsRepeating = true;
		}
		public void PauseMusic()
		{
			if (!BaseData.UsePlayMusic)
			{
				return;
			}
			if (MediaState.Playing == MediaPlayerX.State)
			{
				MediaPlayerX.Pause();
			}
		}
		public void ResumeMusic()
		{
			if (!BaseData.UsePlayMusic)
			{
				return;
			}
			if (MediaState.Paused == MediaPlayerX.State)
			{
				MediaPlayerX.Resume();
			}
		}
		public void StopMusic()
		{
			if (!BaseData.UsePlayMusic)
			{
				return;
			}
			if (MediaState.Playing == MediaPlayerX.State)
			{
				MediaPlayerX.Stop();
			}
		}

		public void StartWinner()
		{
			if (!BaseData.UsePlayMusic)
			{
				return;
			}

			if (MediaState.Playing == MediaPlayerX.State)
			{
				return;
			}

			MediaPlayerX.Play(Assets.Winner);
			MediaPlayerX.IsRepeating = false;
		}
		public void StopWinner()
		{
			if (!BaseData.UsePlayMusic)
			{
				return;
			}
			if (MediaState.Playing == MediaPlayerX.State)
			{
				MediaPlayerX.Stop();
			}
		}

		private static void PlaySoundEffect(SoundEffectType key)
		{
			if (!BaseData.UsePlaySound)
			{
				return;
			}

			SoundEffectInstance value = Assets.SoundEffectDictionary[key];
			value.Play();
		}

	}
}