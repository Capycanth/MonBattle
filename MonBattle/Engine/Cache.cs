using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;

namespace MonBattle.Engine
{
    public class Cache
    {
        private static Cache _singleton;

        private Dictionary<string, Texture2D> _textureCache;
        private Dictionary<string, Song> _songCache;
        private Dictionary<string, SoundEffect> _soundEffectCache;

        public static Cache GetCache()
        {
            if (null == _singleton)
            {
                _singleton = new Cache();
            }
            return _singleton;
        }

        private Cache()
        {
            _textureCache = new Dictionary<string, Texture2D>();
            _songCache = new Dictionary<string, Song>();
            _soundEffectCache = new Dictionary<string, SoundEffect>();
        }

        public void emptyCache()
        {
            // TODO: Add check for carry-over items
            disposeAllSongs();
            disposeAllTextures();
            disposeAllSoundEffects();
        }

        #region Texture Functions
        public void addTexture(Texture2D texture2D, string identifier)
        {
            if (!_textureCache.ContainsKey(identifier))
            {
                _textureCache[identifier] = texture2D;
            }
        }

        public void addTextures(Dictionary<string, Texture2D> valuePairs)
        {
            foreach (KeyValuePair<string, Texture2D> pair in valuePairs)
            {
                addTexture(pair.Value, pair.Key);
            }
        }

        public void disposeTexture(string identifier)
        {
            if (_textureCache.ContainsKey(identifier))
            {
                _textureCache[identifier].Dispose();
            }
        }

        public void disposeAllTextures()
        {
            foreach (Texture2D texture in _textureCache.Values)
            {
                texture.Dispose();
            }
        }

        public Dictionary<string, Texture2D> TextureCache
        {
            get { return _textureCache; }
        }
        #endregion

        #region Song Functions

        public void addSong(Song song, string identifier)
        {
            if (!_songCache.ContainsKey(identifier))
            {
                _songCache[identifier] = song;
            }
        }

        public void addSongs(Dictionary<string, Song> valuePairs)
        {
            foreach (KeyValuePair<string, Song> pair in valuePairs)
            {
                addSong(pair.Value, pair.Key);
            }
        }

        public void disposeSong(string identifier)
        {
            if (_songCache.ContainsKey(identifier))
            {
                _songCache[identifier].Dispose();
            }
        }

        public void disposeAllSongs()
        {
            foreach (Song song in _songCache.Values)
            {
                song.Dispose();
            }
        }

        public Dictionary<string, Song> SongCache
        {
            get { return _songCache; }
        }

        #endregion

        #region SoundEffects

        public void addSoundEffect(SoundEffect soundEffect, string identifier)
        {
            if (!_soundEffectCache.ContainsKey(identifier))
            {
                _soundEffectCache[identifier] = soundEffect;
            }
        }

        public void addSoundEffects(Dictionary<string, SoundEffect> valuePairs)
        {
            foreach (KeyValuePair<string, SoundEffect> pair in valuePairs)
            {
                addSoundEffect(pair.Value, pair.Key);
            }
        }

        public void disposeSoundEffect(string identifier)
        {
            if (_soundEffectCache.ContainsKey(identifier))
            {
                _soundEffectCache[identifier].Dispose();
            }
        }

        public void disposeAllSoundEffects()
        {
            foreach (SoundEffect soundEffect in _soundEffectCache.Values)
            {
                soundEffect.Dispose();
            }
        }

        public Dictionary<string, SoundEffect> SoundEffectCache
        {
            get { return _soundEffectCache; }
        }
        #endregion
    }
}
