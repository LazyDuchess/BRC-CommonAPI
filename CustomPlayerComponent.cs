using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Reptile;
using UnityEngine.UI;

namespace CommonAPI
{
    internal class CustomPlayerComponent : MonoBehaviour
    {
        public Image CustomContextIcon;
        public CustomInteractable CurrentCustomInteractable = null;
        public int CustomInteractableContextAvailable = 0;
        private Player _player;
        public static CustomPlayerComponent Get(Player player)
        {
            return player.GetComponent<CustomPlayerComponent>();
        }

        public static CustomPlayerComponent Attach(Player player)
        {
            var comp = player.gameObject.AddComponent<CustomPlayerComponent>();
            comp._player = player;
            comp.Init();
            return comp;
        }

        void Init()
        {
            if (_player.ui == null)
                return;
            CustomContextIcon = Instantiate(_player.ui.contextTalkIcon, _player.ui.contextTalkIcon.gameObject.transform.parent);
        }
    }
}
