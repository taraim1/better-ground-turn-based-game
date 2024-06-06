
using DG.Tweening.Plugins.Core.PathCore;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using UnityEngine.UI;
public class Character_Screen : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        //����Ʈ�� �رݵ� ĳ���͸� �����Ѵ�
        List<Character> Unlocked_Characters = Unlocked_Character();
        for(int i=0; i<Unlocked_Characters.Count; i++)
        {
            //Char_UI.Char_UI_Set ...�ӽñ� �ӽñ� �ؼ� ���� ���� ������?
        }
        

    }
    List<Character> Unlocked_Character() //�رݵ� ĳ���� ����Ʈ ��ȯ �ڵ�
    {
        //ĳ������ json�����Ͱ� ����� ���
        string dPath = "/Data/CharaterData";
        List<Character> Unlocked_Characters = new List<Character>(); //�رݵ� ĳ���͸� ������ ���

        Character character = new Character();
        //��ο� �ִ� json ���ϵ� ����� ����
        string[] files = Directory.GetFiles(Application.dataPath + dPath, "*.json", SearchOption.TopDirectoryOnly);

        for (int i = 0; i < files.Length; i++)
        {
            string output = File.ReadAllText(files[i]);
            JsonUtility.FromJsonOverwrite(output, character);
            if (character.is_character_unlocked)
            {
                Unlocked_Characters.Add(character);
                //Debug.Log(Unlocked_Characters[i].description);
            }
        }
        return Unlocked_Characters;
    }
 
}
