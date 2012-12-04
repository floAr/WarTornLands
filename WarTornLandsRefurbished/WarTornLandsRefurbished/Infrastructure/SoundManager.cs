using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace WarTornLands.Infrastructure
{
    public class SoundManager
    {
        private static SoundManager _instance = new SoundManager();

        public static SoundManager Instance
        { get { return _instance; } }

        private Dictionary<String, SoundEffect> _soundBank;
        // private List<SoundEffectInstance> _channels;
        private SoundEffectInstance _effectInstance;
        public SoundManager()
        {
            _soundBank = new Dictionary<string, SoundEffect>();
        }

        public void AddSound(String name)
        { this.AddSound(name, name); }

        public void AddSound(String name, String filename)
        {
            SoundEffect se = Game1.Instance.Content.Load<SoundEffect>(filename);
            _soundBank.Add(name, se);
        }
        public void PlaySong(String filename)
        {
            Song song = Game1.Instance.Content.Load<Song>(filename);
            MediaPlayer.Play(song);
        }
        public void StartPlaying(String name)
        {
            if (_soundBank.ContainsKey(name))
            {
                _effectInstance = _soundBank[name].CreateInstance();
                _effectInstance.Play();
            }

        }
    }
}
