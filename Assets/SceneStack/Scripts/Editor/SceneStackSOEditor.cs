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
        [SerializeField] private StyleSheet _styleSheet;
        private VisualElement _root = null;
        private HelpBox _buildSettingWarning = null;
        private void OnEnable()
        {
            EditorBuildSettingsSceneManager.SceneListChanged += SetAllObjectLabelColor;
        }
        private void OnDisable()
        {
            EditorBuildSettingsSceneManager.SceneListChanged -= SetAllObjectLabelColor;
        }
        public override VisualElement CreateInspectorGUI()
        {
            _root = new VisualElement();
            _root.styleSheets.Add(_styleSheet);

            _buildSettingWarning = CreateBuildSettingWarning();
            var baseSceneWarning = CreateBaseSceneWarning();
            var baseSceneField = CreateBaseSceneField(baseSceneWarning);
            var overlaySceneListView = CreateOverlaySceneListView();

            _root.Add(CreateOpenButton());

            _root.Add(_buildSettingWarning);
            _root.Add(baseSceneWarning);
            _root.Add(baseSceneField);
            SetElementActive(_buildSettingWarning, false);

            var box = new Box();

            box.Add(new Label("Overlay Scenes"));
            box.Add(overlaySceneListView);

            box.AddToClassList("scenestack-box");
            overlaySceneListView.AddToClassList("scenestack-list");

            _root.Add(box);

            _root.TrackSerializedObjectValue(serializedObject, (obj) =>
            {
                SetAllObjectLabelColor();
            });

            return _root;
        }

        private void SetAllObjectLabelColor()
        {
            if (_root == null) return;

            bool needWarning = false;
            _root.Query<ObjectField>().ForEach(
                field => {
                    SceneAsset sceneAsset = field.value as SceneAsset;
                    SetObjectLabelColor(field, sceneAsset);

                    string path = AssetDatabase.GetAssetPath(sceneAsset);
                    needWarning |= !EditorBuildSettingsSceneManager.IsEnabled(path);
                });
            SetElementActive(_buildSettingWarning, needWarning);
        }
        void SetObjectLabelColor(ObjectField objectField, SceneAsset sceneAsset)
        {
            Label objectLabel = objectField.Q(className: ObjectField.objectUssClassName).Q<Label>();

            if (sceneAsset == null)
            {
                objectLabel.EnableInClassList("scenestack-missing", true);
                objectLabel.EnableInClassList("scenestack-enabled", false);
                objectLabel.EnableInClassList("scenestack-disabled", false);

                SetElementActive(_buildSettingWarning, true);
            }
            else
            {
                string path = AssetDatabase.GetAssetPath(sceneAsset);

                bool isInBuild = EditorBuildSettingsSceneManager.IsInBuild(path);
                bool isEnabled = EditorBuildSettingsSceneManager.IsEnabled(path);

                objectLabel.EnableInClassList("scenestack-enabled", isEnabled);
                objectLabel.EnableInClassList("scenestack-disabled", isInBuild && !isEnabled);
                objectLabel.EnableInClassList("scenestack-missing", !isInBuild);

                if(!isEnabled) SetElementActive(_buildSettingWarning, true);
            }
        }

        private Button CreateOpenButton()
        {
            void OpenSceneStack()
            {
                var sceneStackSO = (SceneStackSO)target;
                SceneStackEditorUtility.OpenSceneStack(sceneStackSO.CloneSceneStack());
            }
            var openButton = new Button(OpenSceneStack) { text = "OpenScene (Editor Mode)" };
            return openButton;
        }

        private HelpBox CreateBaseSceneWarning()
        {
            var helpBox = new HelpBox("Please make sure to assign the Base Scene", HelpBoxMessageType.Warning);
            return helpBox;
        }
        private HelpBox CreateBuildSettingWarning()
        {
            var helpBox = new HelpBox("Some scenes in SceneStack are not included in build.", HelpBoxMessageType.Warning);
            return helpBox;
        }

        private void SetElementActive(VisualElement e, bool active)
        {
            e.style.display = active ? DisplayStyle.Flex : DisplayStyle.None;
        }
        private ObjectField CreateBaseSceneField(HelpBox warning)
        {
            var baseSceneField = new ObjectField("Base Scene");
            baseSceneField.objectType = typeof(SceneAsset);

            var baseSceneProp = serializedObject.FindProperty("_baseScene");
            var baseSceneAsset = GetSceneAsset(baseSceneProp);

            baseSceneField.SetValueWithoutNotify(baseSceneAsset);
            SetElementActive(warning, baseSceneField.value == null);
            SetObjectLabelColor(baseSceneField, baseSceneAsset);

            baseSceneField.RegisterCallback<ChangeEvent<Object>>((evt) =>
            {
                SetGUID(baseSceneProp, evt.newValue as SceneAsset);
                SetObjectLabelColor(baseSceneField, evt.newValue as SceneAsset);
                serializedObject.ApplyModifiedProperties();
            });

            baseSceneField.TrackPropertyValue(baseSceneProp, (newProp) => {
                baseSceneField.SetValueWithoutNotify(GetSceneAsset(newProp));
                SetElementActive(warning, baseSceneField.value == null);
            });

            return baseSceneField;
        }

        private ListView CreateOverlaySceneListView()
        {
            var overlayListView = new ListView();
            var overlaySceneListProp = serializedObject.FindProperty("_overlayScenes");

            List<SceneAsset> sceneAssets = new();
            SetSourceList(overlaySceneListProp);

            void SetSerializedList()
            {
                overlaySceneListProp.ClearArray();

                for (int i = 0; i < sceneAssets.Count(); ++i)
                {
                    overlaySceneListProp.InsertArrayElementAtIndex(i);
                    var curProp = overlaySceneListProp.GetArrayElementAtIndex(i);

                    SetGUID(curProp, sceneAssets[i]);
                }

                serializedObject.ApplyModifiedProperties();
            }
            void SetSourceList(SerializedProperty prop)
            {
                sceneAssets.Clear();
                for (int i = 0; i < prop.arraySize; ++i)
                {
                    sceneAssets.Add(GetSceneAsset(prop.GetArrayElementAtIndex(i)));
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
                SetSerializedList();
            }
            void OnItemIndexChanged(int oldIndex, int newIndex)
            {
                overlayListView.Rebuild();
                SetSerializedList();
            }
            void OnItemsSourceSizeChanged()
            {
                SetSerializedList();
            }

            void BindItem(VisualElement e, int i)
            {
                var field = e as ObjectField;
                field.SetValueWithoutNotify(sceneAssets[i]);
                SetObjectLabelColor(field, sceneAssets[i]);
                field.RegisterCallback<ChangeEvent<Object>, int>(Callback, i);
            }
            void UnBindItem(VisualElement e, int i)
            {
                var field = e as ObjectField;
                field.UnregisterCallback<ChangeEvent<Object>, int>(Callback);
            }

            overlayListView.itemsSource = sceneAssets;

            overlayListView.makeItem = MakeItem;
            overlayListView.bindItem = BindItem;
            overlayListView.unbindItem = UnBindItem;

            // set serialized list
            overlayListView.itemIndexChanged += OnItemIndexChanged;
            overlayListView.viewController.itemsSourceSizeChanged += OnItemsSourceSizeChanged;

            // set source list
            overlayListView.TrackPropertyValue(overlaySceneListProp,
                (newProp) => { 
                    SetSourceList(newProp);
                    overlayListView.RefreshItems();
                });


            overlayListView.showBorder = true;
            overlayListView.reorderable = true;
            overlayListView.reorderMode = ListViewReorderMode.Animated;
            overlayListView.showAddRemoveFooter = true;
            overlayListView.selectionType = SelectionType.Multiple;

            return overlayListView;
        }


        private string GetGUID(SerializedProperty sceneRef)
        {
            return sceneRef.FindPropertyRelative("_guid").stringValue;
        }
        private void SetGUID(SerializedProperty sceneRef, SceneAsset sceneAsset)
        {
            string GetGUID(SceneAsset sceneAsset)
            {
                if (sceneAsset == null) return null;
                var path = AssetDatabase.GetAssetPath(sceneAsset);
                return string.IsNullOrEmpty(path) ? null : AssetDatabase.AssetPathToGUID(path);
            }
            sceneRef.FindPropertyRelative("_guid").stringValue = GetGUID(sceneAsset);
        }
        private SceneAsset GetSceneAsset(SerializedProperty sceneRef)
        {
            string GetSceneAssetPath(SerializedProperty sceneRef)
            {
                var guid = GetGUID(sceneRef);
                return string.IsNullOrEmpty(guid) ? null : AssetDatabase.GUIDToAssetPath(guid);
            }
            var path = GetSceneAssetPath(sceneRef);
            return (string.IsNullOrEmpty(path)) ? null : AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
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