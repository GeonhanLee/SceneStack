using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Malcha.SceneStack.Editor
{
    [CustomEditor(typeof(SceneStackSO))]
    public class SceneStackSOEditor : UnityEditor.Editor
    {
        private SceneStackSO _sceneStackSO;

        private ObjectField _baseSceneField;
        private ListView _overlaySceneListView;
        private HelpBox _baseSceneWarning;

        private void OnEnable()
        {
            _sceneStackSO = (SceneStackSO)target;

            _baseSceneField = CreateBaseSceneField();
            _overlaySceneListView = CreateOverlaySceneListView();
            _baseSceneWarning = CreateBaseSceneWarning();

            //EditorBuildSettingsSceneManager.SceneListChanged += ;
        }
        private void OnDisable()
        {
            //EditorBuildSettingsSceneManager.SceneListChanged -= ;
        }
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            root.Add(CreateOpenButton());

            root.Add(_baseSceneWarning);
            root.Add(_baseSceneField);

            var box = new Box();
            box.Add(new Label("Overlay Scenes"));
            box.Add(_overlaySceneListView);

            root.Add(box);

            const int padding = 5;

            box.style.paddingTop = padding;
            box.style.paddingBottom = padding;
            box.style.paddingLeft = padding;
            box.style.paddingRight = padding;

            _overlaySceneListView.style.marginTop = padding;

            return root;
        }

        private Button CreateOpenButton()
        {
            void OpenSceneStack()
            {
                SceneStackEditorUtility.OpenSceneStack(_sceneStackSO.CloneSceneStack());
            }
            var openButton = new Button(OpenSceneStack) { text = "OpenScene (Editor Mode)" };
            return openButton;
        }
        private void ResetSceneField(ObjectField objectField, SerializedProperty sceneRefProperty)
        {
            var baseScenePath = GetSceneAssetPath(sceneRefProperty);
            if (string.IsNullOrEmpty(baseScenePath))
            {
                objectField.SetValueWithoutNotify(null);
            }
            else
            {
                objectField.SetValueWithoutNotify(AssetDatabase.LoadAssetAtPath<SceneAsset>(baseScenePath));
            }
        }

        private HelpBox CreateBaseSceneWarning()
        {
            var helpBox = new HelpBox("Please make sure to assign the Base Scene", HelpBoxMessageType.Warning);

            if (_baseSceneField.value == null)
            {
                //EditorGUILayout.HelpBox("Please make sure to assign the Base Scene", MessageType.Warning);
            }
            return helpBox;
        }
        private ObjectField CreateBaseSceneField()
        {
            var baseSceneField = new ObjectField("Base Scene");
            baseSceneField.objectType = typeof(SceneAsset);

            var baseSceneProp = serializedObject.FindProperty("_baseScene");
            ResetSceneField(baseSceneField, baseSceneProp);

            baseSceneField.RegisterCallback<ChangeEvent<Object>>((evt) =>
            {
                baseSceneProp.FindPropertyRelative("_guid").stringValue = GetGUID(evt.newValue as SceneAsset);
                serializedObject.ApplyModifiedProperties();
            });

            baseSceneField.TrackPropertyValue(baseSceneProp, (newProp) => {
                ResetSceneField(baseSceneField, newProp);
            });


            return baseSceneField;
        }

        private ListView CreateOverlaySceneListView()
        {
            var overlayListView = new ListView();
            var overlaySceneListProp = serializedObject.FindProperty("_overlayScenes");

            List<SceneAsset> sceneAssets = new();
            ResetSceneViewList(overlaySceneListProp);

            void SetProperties()
            {
                overlaySceneListProp.ClearArray();

                for (int i = 0; i < sceneAssets.Count(); ++i)
                {
                    overlaySceneListProp.InsertArrayElementAtIndex(i);
                    var curProp = overlaySceneListProp.GetArrayElementAtIndex(i);
                    if (sceneAssets[i] != null)
                    {
                        curProp.FindPropertyRelative("_guid").stringValue = GetGUID(sceneAssets[i]);
                    }
                    else
                    {
                        curProp.FindPropertyRelative("_guid").stringValue = null;
                    }
                }

                serializedObject.ApplyModifiedProperties();
            }
            void ResetSceneViewList(SerializedProperty prop)
            {
                sceneAssets.Clear();
                for (int i = 0; i < prop.arraySize; ++i)
                {
                    var path = GetSceneAssetPath(prop.GetArrayElementAtIndex(i));
                    sceneAssets.Add(AssetDatabase.LoadAssetAtPath<SceneAsset>(path));
                }
            }

            VisualElement MakeItem()
            {
                var field = new ObjectField();
                field.objectType = typeof(SceneAsset);
                return field;
            }
            
            void Callback(ChangeEvent<Object> evt, int i)
            {
                sceneAssets[i] = (SceneAsset)evt.newValue;
                SetProperties();
            }
            void BindItem(VisualElement e, int i)
            {
                var field = e as ObjectField;
                field.SetValueWithoutNotify(sceneAssets[i]);
                field.RegisterCallback<ChangeEvent<Object>, int>(Callback, i);
            }
            void UnBindItem(VisualElement e, int i)
            {
                var field = e as ObjectField;
                field.UnregisterCallback<ChangeEvent<Object>, int>(Callback);
            }

            void OnItemIndexChanged(int oldIndex, int newIndex)
            {
                overlayListView.Rebuild();
                SetProperties();
            }
            void OnItemsSourceSizeChanged()
            {
                SetProperties();
            }

            overlayListView.itemsSource = sceneAssets;

            overlayListView.makeItem = MakeItem;
            overlayListView.bindItem = BindItem;
            overlayListView.unbindItem = UnBindItem;

            overlayListView.itemIndexChanged += OnItemIndexChanged;
            overlayListView.viewController.itemsSourceSizeChanged += OnItemsSourceSizeChanged;

            overlayListView.TrackPropertyValue(overlaySceneListProp,
                (prop) => { 
                    ResetSceneViewList(prop);
                    _overlaySceneListView.RefreshItems();
                });

            overlayListView.reorderable = true;
            overlayListView.reorderMode = ListViewReorderMode.Animated;
            overlayListView.showAddRemoveFooter = true;
            overlayListView.selectionType = SelectionType.Multiple;

            return overlayListView;
        }
        
        /*
        void DrawWarnings()
        {
            if (string.IsNullOrWhiteSpace(baseScene.guid) || baseScene.guid == Guid.Empty.ToString("N"))
            {
                EditorGUILayout.HelpBox("Please make sure to assign the Base Scene", MessageType.Warning);
            }

            //if (!EditorBuildSettingsSceneManager.IsInBuildAndEnabled(baseScene.data.path))
            //{
            //    EditorGUILayout.HelpBox("Base scene is not in build or disabled!", MessageType.Warning);
            //}
        }
        */

        private string GetSceneAssetPath(SerializedProperty sceneRefProperty)
        {
            var guid = sceneRefProperty.FindPropertyRelative("_guid").stringValue;
            return string.IsNullOrEmpty(guid) ? null : AssetDatabase.GUIDToAssetPath(guid);
        }

        private string GetGUID(SceneAsset sceneAsset)
        {
            var path = AssetDatabase.GetAssetPath(sceneAsset);
            return AssetDatabase.AssetPathToGUID(path);
        }

        [MenuItem("Assets/Open Scene Stack", priority = 19)]
        private static void OpenSceneStack()
        {
            SceneStackEditorUtility.OpenSceneStack((Selection.activeObject as SceneStackSO).CloneSceneStack());
        }

        [MenuItem("Assets/Open Scene Stack", priority = 19, validate = true)]
        private static bool ValidateOpenSceneStack()
        {
            return (Selection.activeObject is SceneStackSO);
        }
    }
}