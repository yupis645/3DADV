using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//------------------------------------------------------------------------------------
//                             �����N���X�̒�`������N���X
//
// image��text�̑g�ݍ��킹���I�u�W�F�N�g��string�^�̃e�L�X�g�f�[�^��sprite�̉摜�Ȃ�
//  �����̗v�f���������N���X���`����B
//------------------------------------------------------------------------------------


//========================================================
//      �e�L�X�g�Ɖ摜����ɂ܂Ƃ߂��N���X
//
// UI�v�f�Ȃǂł悭�g�ݍ��킹�Ďg����image��text����ɂ����N���X
//========================================================

[Serializable]
public class UIImageText     //�e�L�X�g�Ɖ摜����ɂ܂Ƃ߂�
{
    public Text text;
    public Image image;
}

//========================================================
//      �e�L�X�g�f�[�^�Ɖ摜�𕡐��ۑ����Ă����N���X
//
// UI�v�f�̍����ւ��Ƃ��Ďg�����Ƃ�z�肵���N���X
//========================================================
[Serializable]
public class TextSpriteData
{
    [TextArea]
    public List<string> Text_string = new List<string>(); // �e�L�X�g�̃��X�g

    public List<Sprite> Sprites = new List<Sprite>(); // �e�L�X�g�̃��X�g

}