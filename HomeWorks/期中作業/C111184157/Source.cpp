#define _USE_MATH_DEFINES
#include <iostream>
#include <vector>
#include <cmath>
#include <algorithm>

using namespace std;

struct Enemy {
    int id;
    double angle;
    long long distSq;
};

int main() {
    int N;
    int prevX = -1, prevY = -1;
    int headVX = 0, headVY = 1;

    while (cin >> N && N != -1) {
        int curX, curY;
        if (!(cin >> curX >> curY)) break;

        if (prevX != -1) {
            int dx_move = curX - prevX;
            int dy_move = curY - prevY;
         
            if (dx_move != 0 || dy_move != 0) {
                headVX = dx_move;
                headVY = dy_move;
            }
        }

        vector<Enemy> enemies(N);
        double baseAngle = atan2((double)headVY, (double)headVX);

        for (int i = 0; i < N; ++i) {
            int ex, ey;
            cin >> ex >> ey;
            enemies[i].id = i;
            int dx = ex - curX;
            int dy = ey - curY;

            double targetAngle = atan2((double)dy, (double)dx);
            double diff = (baseAngle - targetAngle) * 180.0 / M_PI;

            while (diff < 0) diff += 360.0;
            while (diff >= 360.0) diff -= 360.0;

            enemies[i].angle = diff;
            enemies[i].distSq = (long long)dx * dx + (long long)dy * dy;
        }

        sort(enemies.begin(), enemies.end(), [](const Enemy& a, const Enemy& b) {
            if (abs(a.angle - b.angle) > 1e-9) return a.angle < b.angle;
            return a.distSq < b.distSq;
            });

        for (int i = 0; i < N; ++i) {
            cout << enemies[i].id << " ";
        }
        cout << endl;

        prevX = curX;
        prevY = curY;
    }

    return 0;
}