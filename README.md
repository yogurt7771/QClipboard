# q-clipboard-server

Q Clipboard Server是一个跨平台的剪切板同步方案。

使用之前你需要一个服务器来部署服务。

## 服务器端

### docker部署

推荐使用docker来部署，docker-compose.yaml如下：

```yaml
version: '3'

services:
  q-clipboard-server:
    image: patrickwangqy/q-clipboard-server
    restart: unless-stopped
    volumes:
      - ./config:/app/config
    ports:
      - "5000:5000"
```

### 编译部署

1. 安装.net 6.0

   https://dotnet.microsoft.com/en-us/download/dotnet/6.0
2. 执行编译命令

```powershell
dotnet publish -c Release
```

程序会生成在bin/Release/net6.0

3. 运行程序

## 客户端

本着不重复造轮子的思想，所有的客户端均使用已存在的app

### Windows

windows端使用powershell脚本来实现剪切板同步

[Windows客户端](Clients/Windows/q-clipboard-pc)

快捷键可以通过建立开始菜单的快捷方式来设置

开始菜单的路径为

> C:\ProgramData\Microsoft\Windows\Start Menu\Programs

![image](https://user-images.githubusercontent.com/22412010/183232722-b4b911fe-984f-4d86-9ed3-20490bc4ab9b.png)

### Android

Android端使用[http-shortcuts](https://http-shortcuts.rmy.ch/)

1. 增加4个变量

    QClipboardUrl：类型常量，值为服务端地址，例如http://192.168.1.1/Clipboard

    QClipboardUser：类型常量，值为用户名

    QClipboardToken：类型常量，token值要与服务端配置一致

    QClipboardText：类型为剪切板内容

2. 导入配置

    [配置](Clients/Android/shortcuts.json)

### iOS

iOS端使用快捷指令

[获取剪切板](https://www.icloud.com/shortcuts/a8dc7996c8be447a9f362cd81ac43c41)

[推送剪切板](https://www.icloud.com/shortcuts/dc8dc27d7eef462ab1e59b994bf28b5d)

[推送图像](https://www.icloud.com/shortcuts/d19828cd16434b97b07fd664a1b2f60a)

[推送文件](https://www.icloud.com/shortcuts/e1f5bec859ae4764a1ed19999f9e483b)

## 功能列表

* [X] 文本推送、拉取
* [X] 图像推送、拉取
* [X] 文件推送、拉取
* [ ] 剪切板历史
