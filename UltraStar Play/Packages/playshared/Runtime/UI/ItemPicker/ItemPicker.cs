using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemPicker : VisualElement
{
    // UIToolkit factory class
    public new class UxmlFactory : UxmlFactory<ItemPicker, UxmlTraits> {};
    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        // Additional XML attributes
        private readonly UxmlBoolAttributeDescription wrapAround = new() { name = "wrap-around", defaultValue = false};
        private readonly UxmlDoubleAttributeDescription minValue = new() { name = "min-value", defaultValue = double.MinValue};
        private readonly UxmlDoubleAttributeDescription maxValue = new() { name = "max-value", defaultValue = double.MaxValue};
        private readonly UxmlDoubleAttributeDescription stepValue = new() { name = "step-value", defaultValue = 1};
        private readonly UxmlBoolAttributeDescription noPreviousButton = new() { name = "no-previous-button", defaultValue = false};
        private readonly UxmlBoolAttributeDescription noNextButton = new() { name = "no-next-button", defaultValue = false};

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            ItemPicker target = ve as ItemPicker;

            // Read additional attributes from XML
            // In the UIBuilder, the XML attributes and target object fields are synchronized implicitly by name.
            target.WrapAround = wrapAround.GetValueFromBag(bag, cc);
            target.MinValue = minValue.GetValueFromBag(bag, cc);
            target.MaxValue = maxValue.GetValueFromBag(bag, cc);
            target.StepValue = stepValue.GetValueFromBag(bag, cc);
            target.NoPreviousButton = noPreviousButton.GetValueFromBag(bag, cc);
            target.NoNextButton = noNextButton.GetValueFromBag(bag, cc);
        }
    }

    public bool WrapAround { get; set; }
    public double MinValue { get; set; }
    public double MaxValue { get; set; }
    public double StepValue { get; set; }

    public bool NoPreviousButton
    {
        get => !PreviousItemButton.IsVisibleByDisplay();
        set => PreviousItemButton.SetVisibleByDisplay(!value);
    }

    public bool NoNextButton
    {
        get => !NextItemButton.IsVisibleByDisplay();
        set => NextItemButton.SetVisibleByDisplay(!value);
    }

    public Button NextItemButton { get; private set; }
    public Button PreviousItemButton { get; private set; }
    public Label ItemLabel { get; private set; }

    private object control;

    public ItemPicker()
    {
        // Load UXML and add as child element
        string path = "UIDocuments/ItemPicker";
        VisualTreeAsset visualTreeAsset = Resources.Load<VisualTreeAsset>(path);
        if (visualTreeAsset == null)
        {
            Debug.LogError("Could not load " + path);
            return;
        }
        visualTreeAsset.CloneTree(this);

        ItemLabel = this.Q<Label>("itemLabel");
        PreviousItemButton = this.Q<Button>("previousItemButton");
        NextItemButton = this.Q<Button>("nextItemButton");
    }

    public virtual void InitControl(object itemPickerControl)
    {
        if (control != null)
        {
            throw new UnityException("Already initialized");
        }
        control = itemPickerControl;
    }
}
