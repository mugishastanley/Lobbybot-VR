using UnityEngine;
using UnityEngine.UI;

using UnityEditor;

// ---------------
//  String => Int
// ---------------
[UnityEditor.CustomPropertyDrawer(typeof(StringIntDictionary))]
public class StringIntDictionaryDrawer : SerializableDictionaryDrawer<string, int> {
	protected override SerializableKeyValueTemplate<string, int> GetTemplate() {
		return GetGenericTemplate<SerializableStringIntTemplate>();
	}
}
internal class SerializableStringIntTemplate : SerializableKeyValueTemplate<string, int> {}

// ---------------
//  GameObject => Float
// ---------------
[UnityEditor.CustomPropertyDrawer(typeof(GameObjectFloatDictionary))]
public class GameObjectFloatDictionaryDrawer : SerializableDictionaryDrawer<GameObject, float> {
	protected override SerializableKeyValueTemplate<GameObject, float> GetTemplate() {
		return GetGenericTemplate<SerializableGameObjectFloatTemplate>();
	}
}
internal class SerializableGameObjectFloatTemplate : SerializableKeyValueTemplate<GameObject, float> {}

// ---------------
//  Material => Int
// ---------------
[UnityEditor.CustomPropertyDrawer(typeof(MaterialIntDictionary))]
public class MaterialIntDictionaryDrawer : SerializableDictionaryDrawer<Material, int> {
	protected override SerializableKeyValueTemplate<Material, int> GetTemplate() {
		return GetGenericTemplate<SerializableMaterialIntTemplate>();
	}
}
internal class SerializableMaterialIntTemplate : SerializableKeyValueTemplate<Material, int> {}