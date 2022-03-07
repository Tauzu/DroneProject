using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�h���[���������Z�N���X

//���c�͊O���X�N���v�g�iDroneController.cs�j����Apublic�ϐ��ɃA�N�Z�X���čs���B
//�R���g���[���[����̓��͂ɉ������������Z���s���Ĉړ�����B

public class DronePhysics : MonoBehaviour
{
    [System.NonSerialized]
    public float targetHeight;
    [System.NonSerialized]
    public bool isHovering = false;
    [System.NonSerialized]
    public bool isBoosting = false;

    Transform tf;
    Rigidbody rbody;

    struct Blade
    {
        Transform tf;
        int pitchSign;
        int rollSign;

        public Blade(Transform tf, int pitchSign, int rollSign)
        {
            this.tf = tf;
            this.pitchSign = pitchSign;
            this.rollSign = rollSign;
        }

        public void Rotate(float power)
        {
            this.tf.Rotate(new Vector3(0, 100 * power * this.pitchSign, 0));
        }

        public Vector3 Position()
        {
            return this.tf.position;
        }

        public float GetPower(float hoveringPow, float pitchCtrlPow, float rollCtrlPow)
        {
            return hoveringPow + this.pitchSign * pitchCtrlPow + this.rollSign * rollCtrlPow;
        }


    };

    const int numBlade = 4;

    Blade[] bladeArray = new Blade[numBlade];

    float pitch_pre;
    float roll_pre;

    const float baseSpeed = 10f;
    const float coeff = 10f;
    const float Kp = 5f;
    const float decay = 1f;

    Vector3 positionMin = new Vector3(-100f, -10f, -100f);
    Vector3 positionMax = new Vector3(200f, 900f, 300f);

    float maxPower;

    // Start is called before the first frame update
    // Start�AUpdate�֐��Ȃǂ́A�p�������Ǝ����I�ɃI�[�o�[���C�h�����
    protected void InitPhysics()
    {
        tf = this.transform;

        rbody = this.GetComponent<Rigidbody>();
        rbody.centerOfMass = Vector3.zero; //BodyBox�̒��S��S�̂̏d�S�Ƃ���i�u���[�h�̎��ʂ͖����j
        // rbody = tf.Find("BodyMesh").gameObject.GetComponent<Rigidbody>();

        bladeArray[0] = new Blade(tf.Find("blade1"), -1, -1);
        bladeArray[1] = new Blade(tf.Find("blade2"), 1, -1);
        bladeArray[2] = new Blade(tf.Find("blade3"), 1, 1);
        bladeArray[3] = new Blade(tf.Find("blade4"), -1, 1);

        (pitch_pre, roll_pre) = this.GetAttitude();

    }

    // Update is called once per frame
    void Update()
    {
        //PositionClamp();
    }

    protected void PhysicalCalculation(Vector2 targetVector, float inputVertical)
    {
        float inputMagnitude = targetVector.magnitude;

        (float yaw, bool isBacking, float directionCosine) = CalculateYaw(targetVector, inputMagnitude);

        float hoveringPower = HoveringPower(inputVertical);

        (float pitchCtrlPower, float rollCtrlPower) = AttitudeControl(
            inputMagnitude, hoveringPower, isBacking, directionCosine
        );

        maxPower = 0f;
        foreach (Blade blade in bladeArray)
        {
            //Debug.Log(blade.GetPower());
            float power = blade.GetPower(hoveringPower, pitchCtrlPower, rollCtrlPower);

            rbody.AddForceAtPosition(tf.up * coeff * power, blade.Position());

            blade.Rotate(power);

            maxPower = Mathf.Max(maxPower, Mathf.Abs(power));

        }

        // �@�̃��[��]
        Quaternion rot = Quaternion.AngleAxis(10f * yaw, tf.up);
        tf.rotation *= rot;

        if (Input.GetKey(KeyCode.X)) { rbody.AddTorque(-1000f * tf.right); }

        targetHeight += 0.01f * (tf.position.y - targetHeight); //�ڕW�������A�����̍����ɏ����߂Â���i���艻�̂��߁j

    }

    (float, bool, float) CalculateYaw(Vector2 targetVector, float inputMagnitude)
    {
        Vector2 forwardXZ = new Vector2(tf.forward.x, tf.forward.z).normalized;
        Vector2 rightXZ = new Vector2(tf.right.x, tf.right.z).normalized;

        float inner = Vector2.Dot(forwardXZ, targetVector.normalized); //���݂̕����x�N�g���ƁA�ڕW�����x�N�g���Ƃ̓���
        bool isBacking = (inner < -0.1f) ? true : false;

        //���[��]�v����
        float yaw = (isBacking) ? inputMagnitude * Mathf.Abs(-1f - inner)      //�o�b�N��(���ς�-1�ɂȂ邱�Ƃ�ڎw��)
                             : inputMagnitude * Mathf.Abs(1f - inner);     //�O�i��
        if (Vector2.Dot(rightXZ, targetVector) < 0f) yaw *= -1f;    //��]����
        if (isBacking) yaw *= -1f;    //�o�b�N���͉�]����������ɋt�ɂȂ�

        return (yaw, isBacking, inner);

    }

    float HoveringPower(float inputVertical)
    {
        float power;
        if (isHovering && (tf.up.y > 0f))   //�z�o�����O����
        {
            targetHeight += 0.2f * inputVertical;
            //targetHeight += Mathf.Max(0.5f*inputVertical, -0.2f);

            power = Kp * (targetHeight - tf.position.y) - decay * this.rbody.velocity.y;

        }
        else    //�z�o�����OOFF��
        {
            power = inputVertical;

        }

        power = Mathf.Clamp(power, -10f, 10f);

        return power;

    }

    protected void PositionClamp()
    {
        Vector3 modifiedPosition;
        modifiedPosition.x = Mathf.Clamp(tf.position.x, positionMin.x, positionMax.x);
        modifiedPosition.y = Mathf.Clamp(tf.position.y, positionMin.y, positionMax.y);
        modifiedPosition.z = Mathf.Clamp(tf.position.z, positionMin.z, positionMax.z);

        if (modifiedPosition != tf.position)
        {
            tf.position = modifiedPosition;
            rbody.velocity = Vector3.zero;
        }
    }

    float TargetPitchAngle(float inputMagnitude, bool isBacking, float inner)
    {
        float horizontalForwardSPD = CalcHorizontalSpeed(this.rbody.velocity, tf.forward);
        float targetAngle;
        if (inputMagnitude > 0.1f)
        {
            float targetSPD = baseSpeed * inputMagnitude;
            if (isBoosting) targetSPD *= 2f;
            if (isBacking) targetSPD *= -1;
            float difference = (targetSPD - horizontalForwardSPD) / baseSpeed;//�ڕW���x�܂ł̍��̎w�W
            targetAngle = 90f * difference * Mathf.Abs(inner);
            //Debug.Log("ON");
        }
        else
        {
            //�O����������u���[�L
            targetAngle = (isHovering) ? -45f * horizontalForwardSPD / baseSpeed : 0f;
            //Debug.Log("OFF");
        }

        return targetAngle;
    }

    float TargetRollAngle()
    {
        float horizontalRightSPD = CalcHorizontalSpeed(this.rbody.velocity, tf.right);
        //���E���������u���[�L
        float targetAngle = (isHovering) ? 60f * horizontalRightSPD / baseSpeed : 0f;

        if (isBoosting) targetAngle *= 0.2f; //�������̓u���[�L��߂�

        return targetAngle;
    }

    (float, float) AttitudeControl(float inputMagnitude, float hoveringPower, bool isBacking, float inner)
    {
        //pitch[1] = tf.forward.y;
        //roll[1] = tf.right.y;
        (float pitch, float roll) = this.GetAttitude();

        float targetPitchAngle = Mathf.Clamp(TargetPitchAngle(inputMagnitude, isBacking, inner), -60f, 60f);
        targetPitchAngle *= Mathf.Sign(hoveringPower); // �㏸���Ɖ��~���ŌX����ׂ��������t�ɂȂ�
        float targetPitch = targetPitchAngle * Mathf.Deg2Rad;    //�s�b�`�p������
        float pitchControlPOW = Kp * (targetPitch - pitch) - 0.3f * decay * (pitch - pitch_pre) / Time.deltaTime;
        //pitchControlPOW *= 0.1f;
        //Debug.Log(pitch);

        float targetRollAngle = Mathf.Clamp(TargetRollAngle(), -60f, 60f);
        targetRollAngle *= Mathf.Sign(hoveringPower); // �㏸���Ɖ��~���ŌX����ׂ��������t�ɂȂ�
        float targetRoll = targetRollAngle * Mathf.Deg2Rad;    //���[���p������
        float rollControlPOW = Kp * (targetRoll - roll) - 0.3f * decay * (roll - roll_pre) / Time.deltaTime;

        pitch_pre = pitch;
        roll_pre = roll;

        return (pitchControlPOW, rollControlPOW);

    }


    (float, float) GetAttitude()
    {
        //0�`360���̃I�C���[�p���A-180�`+180���Ɋ��Z����
        float pitch = (Mathf.Repeat(tf.rotation.eulerAngles.x + 180, 360) - 180) * Mathf.Deg2Rad;
        float roll = (Mathf.Repeat(tf.rotation.eulerAngles.z + 180, 360) - 180) * Mathf.Deg2Rad;

        return (pitch, roll);
    }

    float CalcHorizontalSpeed(Vector3 velocity3D, Vector3 direction)
    {
        Vector3 HorizontalDirection = new Vector3(direction.x, 0, direction.z).normalized;
        float HorizontalSpeed = Vector3.Dot(velocity3D, HorizontalDirection);
        return HorizontalSpeed;
    }

    public void SwitchHovering(bool flag)
    {
        targetHeight = tf.position.y;
        isHovering = flag;
    }

    public float GetMaxPower()
    {
        return maxPower;
    }

}
