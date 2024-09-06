using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//------------------------------------------------------------------------------------
//                             複合クラスの定義をするクラス
//
// imageとtextの組み合わせたオブジェクトやstring型のテキストデータとspriteの画像など
//  複数の要素を持ったクラスを定義する。
//------------------------------------------------------------------------------------


//========================================================
//      テキストと画像を一つにまとめたクラス
//
// UI要素などでよく組み合わせて使われるimageとtextを一つにしたクラス
//========================================================

[Serializable]
public class UIImageText     //テキストと画像を一つにまとめる
{
    public Text text;
    public Image image;
}

//========================================================
//      テキストデータと画像を複数保存しておくクラス
//
// UI要素の差し替えとして使うことを想定したクラス
//========================================================
[Serializable]
public class TextSpriteData
{
    [TextArea]
    public List<string> Text_string = new List<string>(); // テキストのリスト

    public List<Sprite> Sprites = new List<Sprite>(); // テキストのリスト

}