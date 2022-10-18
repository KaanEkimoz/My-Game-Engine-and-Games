using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Engine
{
    /// <summary>
    /// A class for handling all types of assets: sprites, fonts, sound effects, musics
    /// </summary>
    class AssetManager
    {
        /// <summary>
        /// Monogame ContentManager; loads assets that assigned with pipeline tool
        /// </summary>
        private ContentManager contentManager;

        /// <summary>
        /// Creates a new AssetManager objects
        /// </summary>
        /// <param name="content">A reference to the MonoGame ContentManager to use/param>
        public AssetManager(ContentManager content)
        {
            contentManager = content;
        }

        /// <summary>
        /// Loads and returns the sprite the given asset name
        /// </summary>
        /// <param name="assetName">The name of the asset to load/param>
        /// <returns>A Texture2D object containing the loaded sprite</returns>
        public Texture2D LoadSprite(string assetName)
        {
            return contentManager.Load<Texture2D>(assetName);
        }

        /// <summary>
        /// Loads and returns the font the given asset name
        /// </summary>
        /// <param name="assetName">The name of the asset to load/param>
        /// <returns>A SpriteFont object containing the loaded font</returns>
        public SpriteFont LoadFont(string assetName)
        {
            return contentManager.Load<SpriteFont>(assetName);
        }

        /// <summary>
        /// Loads and plays (only once) the sound effect the given asset name
        /// </summary>
        /// <param name="assetName">The name of the asset to load/param>
        public void PlaySoundEffect(string assetName)
        {
            SoundEffect sfx = contentManager.Load<SoundEffect>(assetName);
            sfx.Play();
        }

        /// <summary>
        /// Loads and plays the song with the given asset name.
        /// </summary>
        /// <param name="assetName">The name of the asset to load.</param>
        /// <param name="isRepeating">Whether or not the song should loop.</param>
        public void PlaySong(string assetName, bool isRepeating)
        {
            MediaPlayer.IsRepeating = isRepeating;
            MediaPlayer.Play(contentManager.Load<Song>(assetName));
        }
    }
}
