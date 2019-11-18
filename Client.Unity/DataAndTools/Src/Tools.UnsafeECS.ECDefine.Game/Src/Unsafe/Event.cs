// Copyright 2019 谭杰鹏. All Rights Reserved //https://github.com/JiepengTan 

using Lockstep.UnsafeECSDefine;

namespace  Lockstep.UnsafeECSDefine {
	
    public class OnSkillFire: IEvent{
   	   	public SkillData SkillData;
    }
  	public class OnSkillDone: IEvent{
   	   	public SkillData SkillData;
    }
}