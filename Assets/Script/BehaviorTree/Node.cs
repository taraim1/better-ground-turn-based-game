using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * 
 * ���� ������ �������
 * 
 */

namespace BehaviorTree 
{

    public class Node // ��� ����� �θ� Ŭ����
    {
        public enum Status 
        { 
            Sucess,
            Failure,
            Running
        }

        public readonly string name;
        protected int current_child_index;
        protected List<Node> children = new List<Node>();

        public Node(string name = "node")
        {
            this.name = name;
        }

        public void AddChild(Node child) => children.Add(child);

        public virtual Status Process() => children[current_child_index].Process();

        public virtual void Reset() 
        { 
            current_child_index = 0;
            foreach (var child in children) child.Reset();
        }
    }

    public class BehaviorTree : Node // �� �ൿ Ʈ���� �ֻ��� ���
    {
        public BehaviorTree(string name) : base(name) { }

        public override Status Process()
        {
            while (current_child_index < children.Count) 
            { 
                Status status = children[current_child_index].Process();

                // ���� or ���� ���̸� �װ� ��ȯ
                if (status != Status.Sucess) 
                {
                    return status;
                }

                // ���� �ڽ� ����
                current_child_index++;
            }

            // ��� ������ ���� ��ȯ
            return Status.Sucess;
        }

    }

    public class Sequence : Node 
    { 
        public Sequence(string name) : base(name) { }

        public override Status Process()
        {
            if (current_child_index < children.Count) 
            {
                switch (children[current_child_index].Process()) 
                {
                    case Status.Running:
                        return Status.Running;
                    case Status.Failure:
                        Reset();
                        return Status.Failure;
                    case Status.Sucess:
                        current_child_index++;
                        return current_child_index == children.Count ? Status.Sucess : Status.Running;
                }
            }

            Reset();
            return Status.Sucess;
        }
    }

    public class Leaf : Node // ������ �����ϴ� ���
    {
        readonly IStrategy strategy;

        public Leaf(string name, IStrategy strategy) : base(name)
        { 
            this.strategy = strategy;
        }

        public override Status Process() => strategy.Process();
        public override void Reset() => strategy.Reset();

    }
}

