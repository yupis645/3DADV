using System;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------------------------------
//                          DI�R���e�i(�C���X�^���X�̊Ǘ�������N���X)
//
// �C���X�^���X���i�[����N���X�B���\�b�h����ăC���X�^���X�̓o�^����n�����邱�Ƃ��ł���
//
//----------------------------------------------------------------------------------------------------------------


public class DIContainer
{
    //registrations�F�^�Ƃ��̌^�̃C���X�^���X�𐶐����邽�߂̃t�@�N�g�����\�b�h���i�[���鎫���B�L�[�͌^ (Type)�A�l�̓C���X�^���X�𐶐����邽�߂̃t�@�N�g�����\�b�h (Func<object>)�B
    private readonly Dictionary<Type, Func<object>> registrations = new Dictionary<Type, Func<object>>();


    //===============================================================
    //              �t�@�N�g�����\�b�h��o�^
    //
    // �^ T �̃C���X�^���X�𐶐�����t�@�N�g�����\�b�h��o�^����B
    //    �p�����[�^�Ffactory - �^ T �̃C���X�^���X�𐶐�����t�@�N�g�����\�b�h�B
    // 1.registrations �����ɁA�L�[�Ƃ��Č^ T�A�l�Ƃ��ăt�@�N�g�����\�b�h��ǉ����܂��B
    //�t�@�N�g�����\�b�h�� Func<object> �Ƃ��ĕۑ�����邽�߁A�L���X�g���s��
    //===============================================================

    public void Register<T>(Func<T> factory) where T : class
    {
        registrations[typeof(T)] = () => factory();
    }

    //===============================================================
    //              �w�肳�ꂽ�^��Ԃ�
    //
    //�o�^���ꂽ�t�@�N�g�����\�b�h���g�p���āA�^ T �̃C���X�^���X�������i�����j���܂��B
    // 1.registrations ��������A�L�[�Ƃ��Č^ T �������B
    // 2.�Ή�����t�@�N�g�����\�b�h��������΁A��������s���ăC���X�^���X�𐶐����ĕԂ��B
    // 3.������Ȃ��ꍇ�i�o�^����Ă��Ȃ��ꍇ�j�A��O���X���[����B
    //===============================================================

    public T Resolve<T>() where T : class
    {
        //�����̒���T���A�����^��������΂�����C���X�^�X�^���X�����ēn��
        if (registrations.TryGetValue(typeof(T), out var factory))
        {
             return (T)factory();
        }

        //������Ȃ��ꍇ�͗�O���X���[����
        throw new InvalidOperationException($"No registration for {typeof(T)}");
    }

    //===============================================================
    //             �o�^�����C���X�^���X�̍X�V
    //
    // �o�^����Ă���C���X�^���X�������̃C���X�^���X�ŏ㏑������
    // 1.�o�^���ꂽ�C���X�^���X�����邩�ǂ������`�F�b�N
    // 2.�o�^���ꂽ�C���X�^���X���㏑��
    //===============================================================
    public void SetInstance<T>(T instance) where T : class
    {
        // 1.�o�^���ꂽ�C���X�^���X�����邩�ǂ������`�F�b�N
        if (!registrations.ContainsKey(typeof(T)))
        {
            throw new InvalidOperationException($"No registration for {typeof(T)}");
        }

        // 2.�o�^���ꂽ�C���X�^���X���㏑��
        registrations[typeof(T)] = () => instance;
    }
    //===============================================================
    //             �o�^���Ă��钆�g�����O�ɕ\��������
    //
    // �f�o�b�N�ȂǂŎg�p����B���ݓo�^���Ă���C���X�^���X���m�F����
    //===============================================================

    public void cheark()
    {
        // Dictionary�̓��e���f�o�b�O���O�ɏo��
        foreach (var kvp in registrations)
        {
            Debug.Log($"Key: {kvp.Key}, Value: {kvp.Value}");
        }
    }
}