#include<list>
#include<unordered_map>
#include<string>
using namespace std;
 
int parent[20000] = { 0, };
 
list<int> allys[625];
list<int> enemys[625];
 
int find(int x)
{
    if (parent[x] == x)
    return x;
    else
        return parent[x] = find(parent[x]);
}
 
void Union(int x, int y)
{
    int a = find(x);
    int b = find(y);
    if (a == b) return;
    parent[b] = a;
    allys[a].splice(allys[a].end(), allys[b]);
    enemys[a].splice(enemys[a].end(), enemys[b]);
}
 
int UnionChk(int x, int y)
{
    int a = find(x);
    int b = find(y);
    if (a == b) return 1;
    else return 0;
}
 
int enemyChk(int x, int y)
{
    int a = find(x);
    int b = find(y);
    for (auto enemy : enemys[b])
    {
        if (find(enemy) == a) return 1;
    }
    return 0;
}
 
void makeEnemy(int x, int y)
{
    int a = find(x);
    int b = find(y);
    if (a == b) return;
    if (enemyChk(x, y)) return;
    enemys[a].push_back(b);
    enemys[b].push_back(a);
}

struct point
{
    int x;
    int y;
    int sol;
};
 
int m_size;
int map[25][25];
unordered_map<string, int> monarchs;
point land[20000] = { 0, };
int cnt = 0;
void init(int N, int mSoldier[25][25], char mMonarch[25][25][11])
{
    m_size = N;
    int index = 0;
    monarchs.clear();
    for (int i = 0; i < N; i++)
    {
        for (int j = 0; j < N; j++)
        {
            map[i][j] = index;
            monarchs[mMonarch[i][j]] = index;
            land[index].x = i;
            land[index].y = j;
            land[index].sol = mSoldier[i][j];
            parent[index] = index;
            allys[index].clear();
            enemys[index].clear();
            allys[index].push_back(index);
            index++;
        }
    }
    cnt = index;
}
 
void destroy()
{
 
}
 
int ally(char mMonarchA[11], char mMonarchB[11])
{
    int x = monarchs[mMonarchA];
    int y = monarchs[mMonarchB];
 
    if (UnionChk(x, y)) return -1;
    if (enemyChk(x, y)) return -2;
    Union(x, y);
    return 1;
}
 
int attack(char mMonarchA[11], char mMonarchB[11], char mGeneral[11])
{
    int x = monarchs[mMonarchA];
    int y = monarchs[mMonarchB];
 
    int px = find(x);
    int py = find(y);
    if (UnionChk(x, y)) return -1;
 
    int bx = land[y].x;
    int by = land[y].y;
 
    int xs = max(0, bx - 1);
    int ys = max(0, by - 1);
    int xe = min(m_size - 1, bx + 1);
    int ye = min(m_size - 1, by + 1);
 
    bool combat = false;
    for (int i = xs; i <= xe; i++)
    {
        for (int j = ys; j <= ye; j++)
        {
            if (find(map[i][j]) == px)
            {
                combat = true;
                i = xe + 1;
                break;
            }
        }
    }
 
    if (combat == false) return -2;
 
    int solx = 0;
    int soly = 0;
    for (int i = xs; i <= xe; i++)
    {
        for (int j = ys; j <= ye; j++)
        {
            int a = find(map[i][j]);
            if (a == px)
            {
                int temp = land[map[i][j]].sol;
                temp /= 2;
                land[map[i][j]].sol -= temp;
                solx += temp;
            }
            else if (a == py)
            {
                int temp = land[map[i][j]].sol;
                if(map[i][j] != y) temp /= 2;
                land[map[i][j]].sol -= temp;
                soly += temp;
            }
        }
    }
 
    makeEnemy(px, py);
    if (solx > soly)
    {
        monarchs.erase(mMonarchB);
        land[y].sol = solx - soly;
        land[cnt] = land[y];
        map[land[cnt].x][land[cnt].y] = cnt;
        parent[cnt] = px;
        monarchs[mGeneral] = cnt;
        allys[px].push_back(cnt);
        allys[py].remove(y);
        cnt++;
        return 1;
    }
 
    land[y].sol = soly - solx;
    return 0;
}
 
int recruit(char mMonarch[11], int mNum, int mOption)
{
    if (mOption == 0)
    {
        land[monarchs[mMonarch]].sol += mNum;
        return land[monarchs[mMonarch]].sol;
    }
    else
    {
        int ret = 0;
        int x = find(monarchs[mMonarch]);
        for(auto it : allys[x])
        {
            land[it].sol += mNum;
            ret += land[it].sol;
        }
        return ret;
    }
}