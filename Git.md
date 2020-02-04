# Git
# Git Guide  
> ### git的三种方式
> > + 只在本地使用；
> > + 将本地仓库上传到Github；
> > + 下载GitHub上的仓库；

### 1、只在本地使用  
> **在Git Bush上输入命令**  
>> - mkdir git-demo-1 ——创建一个目录（git-demo-1，可自定义）
>> - cd git-demo-1 ——进入目录
>> - git init —— 在git-demo-1里创建一个 .git 目录；
>> - touch xxx/mkdir  xxx/... —— 创建文件或者目录（可进行编辑）；
>> - git status -sb —— 显示当前所有文件的状态（可不执行此操作）
>> - git add xxx/ git add . —— 将文件添加到暂存取，在xxx处输入文件名，前者添加指定文件，后者一次性全部添加。
>> - git commit -m "xxx" —— 将上步add过的内容正式提交到本地仓库，.git是本地仓库，在xxx处了输入备注信息；
>> - git log —— 可查看历史上的变动（可不执行该操作）；
>> - 如果有文件有新的变动，我们只需要依次执行git add和git commit -m 'xxx' 两个命令就可以。

### 2、将本地仓库上传到Github 
> + 在Github上新建一个空仓库，名字随意，一般可以跟本地目录名一致；
> + 点SSH按钮，按提示代码操作；
**如果没有本地仓库：**
> >+ git init;
> >+ git add README.md;
> >+ git commit -m "first commit";
**如果有本地仓库：**
> >+ git remote add origin git@github.com:(在Github上复制仓库地址);
> >+ git push -u origin master;

### 3、直接在Github上创建一个仓库，然后下载到本地
> + 在GitHub上新建一个仓库git-demo-2,这次不创建空仓库了，而是自带README和Lisenece的仓库；
> + 点击在页面中的[clone or down load];
> + 确保弹出的地址是SSH地址；
> + 打开git bash，找一个安全的目录；
> + 运行 git clone address；
> + 运行完就OK；

### 如何更新上传
> git add xxx
> git commit -m "xxx"
> git pull
> git push

### 其他
> + git remote add origin address :将本地仓与远程仓关联；
> + git remote aet-url origin address 挽救上一步；


[Git Flow](
http://www.ruanyifeng.com/blog/2015/12/git-cheat-sheet.html)

### 一、新建代码库  
> + git init  *在当前目录常见一个Git代码库* 
> + git init [project-name]*新建一个目录，并初始化为git代码库*
> + git clone [url]*下载一个项目和他的整个代码历史*


### 二、配置
**Git的配置文件为.gitconfig,可以在用户主目录下（全局配置），也可以在项目目录下（项目配置）**
>+ git config --list *显示当前的Git配置*
>+ git config -e[--global]*编辑Git配置文件*
>+ git config[--global] user.name "[name]"
>+ git config [--g;obal] user.email "[email address]"

### 三、增加/删除文件
>+ git add [file1] [file2]*指定文件添加到暂缓区*
>+ git add [dir]*指定目录添加到暂存区*
>+ git add *添加当前目录的所有文件到暂存区*
>+ git add -p *添加每个变化钱，偶读会要求确认，对于同一个文件的多出吧电话，可以实现分次提交*
>+ git rm [file1] [file2]*删除工作区文件，并且将这次删除放入暂存区*
>+ git rm --cached [file]*停止追踪指定文件，但该文件会保留在工作区*
>+ git mv [file-original] [file-renamed] *改名文件，并将这个改名放入暂存区*

### 四、代码提交
> + git commit -m [message]*提交到工作区*
> + git commit [file1] [file2] ... -m [message]*将指定文件提交到工作区*
> + git commit -a *提交到工作区自上次commit之后的变化，直接到仓库区*
> + git commit -v *提交时显示所有的diff信息*
> + git commit --amend -m [message] *使用一次新的commit，代替上一次的提交，如果代码没有任何新的变化，则用来改写上次一commit的提交的信息*
> + git commit --amend [file1] [file2]...*重做上一侧的commit。并包括指定文件袋额新变化*

### 五、分支
>+ git branch *列出所有分支*
>+ git branch -r *列出所有远程分支*
>+ git branch -a *列出所有本地分支和远程峰分支*
>+ git branch [branch-name]*新建一个分支，但依然停留在当前分支*
>+ git checkout -b [branch] *新建一个分支，并切换到该分支*
>+ git branch --track [branch] [remote-branch]*新建一个分支，与指定的远程分支建立追踪关系*
>+ git branch [branch] [commit] *新建一个分支，指向指定commit*
>+ git checkout [branch-name]*切换到指定分支，并更新工作区*
>+ git checkout - *切换上一个分支*
>git branch --set-upstream [branch] [remote-branch] *简历追踪关系，在现有分支与指定远程分支之间*
>+ git merge [branch] *合并指定分支到当前分支*
>+ git cherry-pick [commit] *选择一个commit，合并当前分支*
>+ git branch -d [branch-name]*删除分支*
>+ git push origin --delete [branch-name]*删除远程分支*
>+ git branch -dr [remote/branch]

### 六、标签
> - git tag *列出所有tag*
> - git tag [tag] *新建一个tag在当前commit*
> - git tag [tag] [commit]*新建一个tag指定commit*
> - git tag -d [tag]*删除本地tag*
> git push origin :refs/tags/[tagName]*删除远程tag*
> - git show [tag] *查看tag信息*
> - git push [remote][tag]*提交指定tag*
> - git push [remote] --tags *提交所有tag*
> - git checkout -b [branch] [tag]*新建一个分支，指定某个tag*

### 七、查看信息
> - git status
> - git log
> - git log --start *显示commit历史，以及每次commit发生变更的文件*
> - git log -S [keyword] *搜索提交，历史，根据关键词*
> - git log [tag] HEAD --pretty=format:%s *显示某个commit之后所有变动，其提交说明必须符合搜索条件*
> - git log -follow [file]
> - git whatchanged [file]*显示某个文件的版本历史，包括文件改名*
> - git log -p [file]*显示指定文件相关的每一次的diff*
> - git shortlog -sn*显示所有提交过的用户，按提交次数排序*
> - git log 5 --pretty --oneline*显示过去5次提交*
> - git blame [file]*显示指定文件时什么人在什么时候创建的*
> - git diff *显示暂存区和工作区的差异* 
> - git diff HEAD*显示工作区与当前分支最新commit的差异*
> - git diff --cached[file]*显示工作区与上一个commit的差异*
> - git diff --shortstat "@{0 day ago}"*显示今天写了多少行代码*
> - git show [commit]*显示某次提交的元数据和内容变化*
> - git show --name-only [commit]*显示某偶次提交发生变化的文件*
> git show [commit]:[filename]*显示某次提交时，某个文件的内容*
> - git reflog*显示当前分支的最近几次提交*

### 八、远程同步 
>- git fetch [remote]*下载远程仓库的所有变动*
>- git remote -v *显示所有远程仓库*
>- git remote show [remote]*显示某个仓库的信息*
>- git remote add [shortname] [url] *增减一个新的远程仓，并命名*
>- git pull [remote] --force*强行推送当前分支到远程仓库，即使有冲突*
>- git push [remote] --all *推送所有分支到远程仓库*

### 九、撤销
> - git checkout [file] *恢复暂存区的指定文件到工作区*
> - git checkout [commit] [file]*恢复某个commit的指定文件到暂存区和工作区*
> - git checkout*恢复暂存区的所有文件到工作区*
> - git reset [file]*重置暂存区的指定文件，与上一次commit保持一致，但是工作区不变*
> - git reset --hard *重置暂存区与工作区，与上一次commit保持一致*
> - git reset --hard [commit]*重置当前分支的HEAD为指定commit，同时重置暂存区，与指定commit一致*
> git reset --keep [commit]*重置当前HEAD为指定commit，但保持暂存区和工作区不变*
> - git revert [commit]*新建一个commit，用来撤销指定commit，后者的所有变化豆浆被前者抵消，并且应用到当前分支*
> - git stash *暂时将未提交的变化移除，小猴再移入*
> - git stash pop

### 十、其他
>- git archive *生成一个可供发布的压缩包*