﻿using System;
using ETModel;
using UnityEngine;

namespace ETHotfix
{
    [UIFactory(UIType.UILobby)]
    public class UILobbyFactory : IUIFactory
    {
        public UI Create(Scene scene, string type, GameObject gameObject)
        {
	        try
	        {
				ResourcesComponent resourcesComponent = ETModel.Game.Scene.GetComponent<ResourcesComponent>();
		        resourcesComponent.LoadBundle($"{type}.unity3d");
				GameObject bundleGameObject = resourcesComponent.GetAsset<GameObject>($"{type}.unity3d", $"{type}");
				GameObject lobby = UnityEngine.Object.Instantiate(bundleGameObject);
				lobby.layer = LayerMask.NameToLayer(LayerNames.UI);
				UI ui = ComponentFactory.Create<UI, GameObject>(lobby);

				ui.AddComponent<UILobbyComponent>();
				return ui;
	        }
	        catch (Exception e)
	        {
				Log.Error(e.ToStr());
		        return null;
	        }
		}

	    public void Remove(string type)
	    {
			ETModel.Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle($"{type}.unity3d");
		}
    }
}