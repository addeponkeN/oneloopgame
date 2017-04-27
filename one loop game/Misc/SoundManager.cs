using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace one_loop_game
{
    public class SoundManager
    {

        public SoundEffect songForest;
        public SoundEffectInstance songForestInstance;

        public SoundEffect chop1;
        public SoundEffectInstance chop1Instance;
        public SoundEffect chop3;
        public SoundEffectInstance chop3Instance;

        public SoundEffect yes1;
        public SoundEffectInstance yes1Instance;

        public SoundEffect hammer1;
        public SoundEffectInstance hammer1Instance;

        public static bool MusicOn { get; set; } = true;
        public static bool SoundOn { get; set; } = true;
        public static float Volume { get; set; }

        public SoundManager()
        {
            Volume = .25f;
        }
        public void Load(ContentManager content)
        {
            songForest = content.Load<SoundEffect>("forest");
            songForestInstance = songForest.CreateInstance();
            songForestInstance.Volume = 0.5f;

            chop1 = content.Load<SoundEffect>("chop1");
            chop1Instance = chop1.CreateInstance();
            chop1Instance.Volume = 0.20f;

            chop3 = content.Load<SoundEffect>("chop3");
            chop3Instance = chop3.CreateInstance();
            chop3Instance.Volume = 0.20f;

            yes1 = content.Load<SoundEffect>("yes");
            yes1Instance = yes1.CreateInstance();
            yes1Instance.Volume = 0.5f;

            hammer1 = content.Load<SoundEffect>("hammer1");
            hammer1Instance = hammer1.CreateInstance();
            hammer1Instance.Volume = 0.5f;

        }

        public static void PlayMusicLooped(SoundEffectInstance s)
        {
            if (MusicOn)
            {
                s.IsLooped = true;
                if (s.State != SoundState.Playing)
                    s.Play();
            }
        }

        public static void PlaySoundOnce(SoundEffectInstance i)
        {
            if (SoundOn)
                i.Play();
        }
    }
}
