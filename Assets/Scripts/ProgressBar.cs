using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {


	protected float maxValue;
    protected float minValue;
    protected float currentValue;
    protected RectTransform fillTransform;
    protected float fillMaxWidth;
    public Text barText;
    public bool reversed = false;


	public void SetInitialValues(float max,float min, float initial){
		fillTransform= transform.GetChild(0).GetComponent<RectTransform>();
		maxValue= max;
		minValue= min;
		currentValue= initial;
		fillMaxWidth= fillTransform.sizeDelta.x;

        if (barText != null)
        {
            barText.text = initial + "/" + maxValue;
        }
    }


    public void UpdateMaxValue(float val)
    {
        maxValue = val;
        UpdateBar(currentValue);
    }

	public virtual void UpdateBar(float newVal){

		currentValue= newVal;
		if(newVal<minValue){
			currentValue=minValue;
		}
        //		Debug.Log("Updte Lifebar "+((currentValue-minValue)/(maxValue-minValue)));
        if (!reversed)
        {
            fillTransform.sizeDelta = new Vector2(((currentValue - minValue) / (maxValue - minValue)) * fillMaxWidth, fillTransform.sizeDelta.y);

        }
        else
        {
            fillTransform.sizeDelta = new Vector2(fillMaxWidth - (((currentValue - minValue) / (maxValue - minValue)) * fillMaxWidth), fillTransform.sizeDelta.y);
        }


        if (fillTransform.sizeDelta.x < 0)
        {
            fillTransform.sizeDelta = new Vector2(0.01f, fillTransform.sizeDelta.y);
        }
        if (barText != null)
        {
            barText.text = currentValue + "/" + maxValue;
        }
	}


}
