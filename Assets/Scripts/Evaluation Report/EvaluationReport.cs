using UnityEngine;

public class EvaluationReport : MonoBehaviour
{
    private int currentBlockIndex;
    private ReportBlock currentBlock;
    private ReportBlock[] blocks;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        blocks = GetComponentsInChildren<ReportBlock>(includeInactive: true);
        SetCurrentBlock(0);
    }

    private void SetCurrentBlock(int index)
    {
        currentBlockIndex = index;
        
        for(int i = 0; i<blocks.Length;i++)
        {
            if(i < currentBlockIndex)
            {
                blocks[i].SetValidated();
            }
            else if(i > currentBlockIndex)
            {
                blocks[i].SetDisabled();
            }
            else // equal to currentBlockIndex
            {
                blocks[i].SetCurrent();
                blocks[i].Validated += OnBlockValidated;
            }
        }
    }

    private void OnBlockValidated(ReportBlock block)
    {
        block.Validated -= OnBlockValidated;
        SetCurrentBlock(currentBlockIndex + 1);
    }
}
