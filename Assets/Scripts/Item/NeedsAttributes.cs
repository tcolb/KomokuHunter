using System;
using System.Collections.Generic;
using UnityEngine;


public class NeedsAttributes
{
	public enum AttributeColor
	{
		Red,
		Blue,
		Green,
		Gray
	}

	public enum AttributeTexture
	{
		Polkas,
		Checkered,
		Stars,
	}

	public enum AttributeMesh
	{
		Cellphone,
		Wallet,
		Stapler,
		Bottle,
		Plant,
		Umbrella,
		Sword
	}

	// Primary fields
	public AttributeColor Color;
	public AttributeTexture Texture;
	public AttributeMesh Model;

	public void Randomize()
	{
		Color = RandomEnum<AttributeColor>();
		Texture = RandomEnum<AttributeTexture>();
		//Model = RandomEnum<AttributeMesh>();
	}



	// Mapping
	static Dictionary<Type, Dictionary<int, UnityEngine.Object>> TypeEnumResourceMapping;
	
	public static void SetEnumAttributeMaps()
	{
		TypeEnumResourceMapping = new Dictionary<Type, Dictionary<int, UnityEngine.Object>>();

		SetEnumResourceMap<AttributeTexture, Texture2D>(Main.Instance.LostItemTextures);
		SetEnumResourceMap<AttributeColor, AttributeColorWrapper>(Main.Instance.LostItemColors);
		SetEnumResourceMap<AttributeMesh, SpawnedEntity>(Main.Instance.LostItemMeshes);
	}

	// Helpers
	static void SetEnumResourceMap<T, F>(IList<F> resources) where T : IConvertible where F : UnityEngine.Object
	{
		if (!TypeEnumResourceMapping.TryGetValue(typeof(T), out var enumResourceMapping))
		{
			enumResourceMapping = new Dictionary<int, UnityEngine.Object>();
			TypeEnumResourceMapping.Add(typeof(T), enumResourceMapping);
		}

		foreach (var resource in resources)
		{
			var attributeEnum = (int) Enum.Parse(typeof(T), resource.name);
			if (!enumResourceMapping.TryGetValue(attributeEnum, out _))
			{
				enumResourceMapping.Add(attributeEnum, resource);
			}
			else
			{
				Debug.LogError("[NeedsAttributes] Tried to set pre existing enum mapping... something is messed up :)");
			}
		}
	}

	static F GetResourceFromEnum<T, F>(int enumValue) where T : IConvertible where F : UnityEngine.Object
	{
		if (!TypeEnumResourceMapping.TryGetValue(typeof(T), out var enumResourceMapping))
		{
			Debug.LogError("[NeedsAttribtues] Could not get EnumResourceMap, have resource maps not been set yet?");
			return null;
		}

		if (!enumResourceMapping.TryGetValue(enumValue, out var resource))
		{
			Debug.LogError("[NeedsAttribtues] Could not get resource from EnumResourceMap, have resource maps not been set yet?");
			return null;
		}

		return (F) resource;
	}

	public SpawnedEntity GetModel()
	{
		return GetResourceFromEnum<AttributeMesh, SpawnedEntity>((int)Model);
	}

	public void ApplyToMaterial(Material material)
	{
		var texture = GetResourceFromEnum<AttributeTexture, Texture2D>((int)Texture);
		material.enableInstancing = true; // this may be be desired
		material.mainTexture = texture;
		material.mainTextureScale = new Vector2(5, 5);
		material.color = GetResourceFromEnum<AttributeColor, AttributeColorWrapper>((int)Color).Color;
	}

	public static T RandomEnum<T>() where T : struct, IConvertible
	{
		var vals = Enum.GetValues(typeof(T));
		return (T) vals.GetValue(Main.Instance.Rand.Next(vals.Length)); 
	}

	public override bool Equals(object other)
	{
		if (!(other is NeedsAttributes otherAttributes))
		{
			return false;
		}

		return Color == otherAttributes.Color && Model == otherAttributes.Model && Texture == otherAttributes.Texture;
	}
}