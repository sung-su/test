#include<unordered_map>
 
#define log //printf
#define MAX_SIZE 100001 // 모든 연산 횟수의 합은 100,000이하
 
int heap[MAX_SIZE];
int heapSize = 0;
 
using namespace std;
unordered_map<int, int> map;
 
struct NODE
{
    int key;
    int value;
    int pos; // heap position
};
NODE node[MAX_SIZE];
int nodeidx;
 
void heapInit(void)
{
    heapSize = 0;
}
 
int compare(int a, int b)
{
    if (node[a].value > node[b].value)
        return true;
    else
        return false;
}
 
void heapupdate(int current)
{
    while (current > 0 && compare(heap[current], heap[(current - 1) / 2]))
    {
        int temp = heap[(current - 1) / 2];
        heap[(current - 1) / 2] = heap[current];
        heap[current] = temp;
 
        temp = node[heap[(current - 1) / 2]].pos;
        node[heap[(current - 1) / 2]].pos = node[heap[current]].pos;
        node[heap[current]].pos = temp;
 
        current = (current - 1) / 2;
    }
}
 
int heapPush(int value)
{
    heap[heapSize] = value;
 
    node[heap[heapSize]].pos = heapSize;
    heapupdate(heapSize);
 
    heapSize = heapSize + 1;
 
    return 1;
}
 
void heapdowndate(int current)
{
    while (current * 2 + 1 < heapSize)
    {
        int child;
        if (current * 2 + 2 == heapSize)
        {
            child = current * 2 + 1;
        }
        else
        {
            child = compare(heap[current * 2 + 1], heap[current * 2 + 2]) ? current * 2 + 1 : current * 2 + 2;
        }
 
        if (compare(heap[current], heap[child]))
        {
            break;
        }
 
        int temp = heap[current];
        heap[current] = heap[child];
        heap[child] = temp;
 
        temp = node[heap[current]].pos;
        node[heap[current]].pos = node[heap[child]].pos;
        node[heap[child]].pos = temp;
 
        current = child;
    }
}
 
int heapPop(int *value)
{
    *value = heap[0];
    heapSize = heapSize - 1;
 
    heap[0] = heap[heapSize];
    node[heap[0]].pos = 0;
 
    heapdowndate(0);
 
    return 1;
}
 
void delheap(int idx)
{
    int heapidx = node[idx].pos;
 
    heapSize = heapSize - 1;
 
    heap[heapidx] = heap[heapSize];
    node[heap[heapidx]].pos = heapidx;
 
    heapupdate(heapidx);
    heapdowndate(heapidx);
}
 
void init() {
    nodeidx = 0;
    map.clear();
 
    heapInit(); // 중요!!
}
 
void push(int key, int value) {
    map[key] = nodeidx; 
 
    node[nodeidx].key = key;
    node[nodeidx].value = value;
 
    heapPush(nodeidx);
 
    nodeidx++;
}
 
int storeidx[11];
int top(int K) {
 
    int ans = 0;
    for (int i = 0; i < K; i++)
    {
        int idx;
        heapPop(&idx);
 
        storeidx[i] = idx;
 
        ans += node[idx].value;
    }
 
    for (int i = 0; i < K; i++)
    {
        heapPush(storeidx[i]);
    }
 
    return ans;
}
 
void erase(int key) {
 
    log("@@erase - %d\n", key);
 
    int idx = map[key];
 
    delheap(idx);
}
 
void modify(int key, int value) {
 
    log("@@modify - %d, %d\n", key, value);
 
    int idx = map[key];
 
    node[idx].value = value;
 
    // modify -> update!! 필요
    int heapidx = node[idx].pos;
 
    heapupdate(heapidx);
    heapdowndate(heapidx);
}