using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTest: MonoBehaviour
{

    public Material defMaterial;
    public Transform characterFace;
    public Material lightMaterial;
	public Renderer rightMark;
	public Renderer leftMark;
	public Renderer upMark;
	public Renderer downMark;
    public ParticleSystem myParticleSystem;



	// Start is called before the first frame update
	void Start()
    {
        
    }

    void DoEmit(Transform markPosition)
    {
        var emitParams = new ParticleSystem.EmitParams();
        emitParams.applyShapeToPosition = true;
        //emitParams.position = markPosition.position;
        //myParticleSystem.Emit(emitParams, 50);
        myParticleSystem.Play();
    }


        // Update is called once per frame
        void Update()
    {
        float faceRot = characterFace.rotation.eulerAngles.y;
        faceRot = faceRot > 180 ? faceRot - 360 : faceRot;

        float faceRot2 = characterFace.rotation.eulerAngles.x;
        faceRot2 = faceRot2 > 180 ? faceRot2 - 360 : faceRot2;



        if (Input.GetKey(KeyCode.RightArrow))
        {
            if(faceRot > -30)
            {
                characterFace.Rotate(0,-5,0);

            }
            else
            {
				rightMark.material = lightMaterial;
                DoEmit(rightMark.transform);
            }
 
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (faceRot < 30)
            {
                characterFace.Rotate(0, 5, 0);

            }
            else
            {
                leftMark.material = lightMaterial;
            }

        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (faceRot2 < 30)
            {
                characterFace.Rotate(5, 0, 0);
            }
            else
            {
                upMark.material = lightMaterial;
            }

        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (faceRot2 > -30)
            {
                characterFace.Rotate(-5, 0, 0);
            }
            else
            {
                downMark.material = lightMaterial;
            }

        }

        if(!Input.anyKey)
        {
            //characterFace.rotation = Quaternion.Euler(0, 0, 0);
            rightMark.material = defMaterial;
            leftMark.material = defMaterial;
            upMark.material = defMaterial;
            downMark.material = defMaterial;
        }

    }
}
