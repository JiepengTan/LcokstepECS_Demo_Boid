#  UnsafeECS Demo Boid

#### **简介**
UnsafeECS  一个基于c# 指针和结构体 的帧同步框架，使用于超大型场景的帧同步游戏
优点：
- 运行速度快，使用指针,和结构体，基本无gc , PureMode 都比Entitas 快两倍，Burst Mode，快四倍以上
- 内存紧凑，预测回滚是否帧状态拷贝快 7000 只鱼的状态拷贝只消耗0.3ms
- API 和 UNITY ECS 非常相似，可以使用同一种编程范式来编写 logic 层 和 view 层
- 无缝兼容UnityECS ，使用条件宏可以切换两种模式，
	- PureMode:纯代码形式，可以直接在服务器中运行逻辑，不依赖Unity 
	- Burst Mode: 模式，直接生成适配Unity ECS Burst+job框架代码的代码，进一步提升运行速度



#### **运行**
1. 运行 Assets/Boids/Scenes/UnsafeBoid.Unity  运行UnsafeECS 实现的Boid 场景
	- 两种模式 添加红 UNING_UNITY_BURST_JOB 开启 Burst 加速的Unsafe ECS 模式  
2. 运行 Assets/Boids/Scenes/BoidExample.Unity  运行Unity 官方实现的Boid 场景  （ 需要开启条件宏 USING_UNITY_BOID）
3. [视频][2]


#### **场景图：** 
<p align="center"> <img src="https://github.com/JiepengTan/JiepengTan.github.io/blob/master/assets/img/blog/UnsafeECS/Boidgif.gif?raw=true" width="512"/></p>



#### **None：** 

 [1]: https://github.com/JiepengTan/LockstepEngine
 [2]: https://www.bilibili.com/video/av74152979?spm_id_from=333.171.b_686f6d655f636f6d6d656e745f6c697374.2
 