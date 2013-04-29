#include "uyvy2rgb.h"

#include <QtCore/qmath.h>

UYVY2RGB::UYVY2RGB() : m_bits(NULL), m_width(0), m_height(0)
{
    for (int y = 0; y < 255; y++)
        for (int x = 0; x < y; x++)
            {
            int val = (int)qSqrt(x*x+y*y);
            val = val > 255 ? 255 : val;
            sqrt_table[x][y] = val;
            sqrt_table[y][x] = val;
            }
}

UYVY2RGB::~UYVY2RGB()
{
}

unsigned char* UYVY2RGB::bits()
{
    return m_bits;
}

int UYVY2RGB::width()
{
    return m_width;
}

int UYVY2RGB::height()
{
    return m_height;
}

void UYVY2RGB::initData(const QVideoFrame &source)
{
    if (m_bits == NULL || source.width() != m_width || source.height() != m_height)
    {
        if (m_bits != NULL)
            delete[] m_bits;

        m_width = source.width();
        m_height = source.height();
        m_bits = new unsigned char[m_width * m_height * 4];
    }

    unsigned char *dst = m_bits + m_width * 4;
    const unsigned char *src = source.bits() + m_width * 2;


    for (int y = 1; y < m_height-1; ++y) {
        unsigned char *d_x = dst + 4;
        const unsigned char *s_x = src + 2 + 1;
        dst = dst + m_width * 4; // four bytes per pixel
        src = src + m_width * 2; // two bytes per pixel

        for (int x = 1; x < m_width-1; ++x)
        {
            int gx = qAbs(-(*(s_x-m_width*2-2)) - *(s_x-2)*2 - *(s_x+m_width*2-2) + *(s_x-m_width*2+2) + *(s_x+2)*2 + *(s_x+m_width*2+2));
            int gy = qAbs(-(*(s_x-m_width*2-2)) - *(s_x-m_width*2)*2 - *(s_x-m_width*2-2) + *(s_x+m_width*2-2) + *(s_x+m_width*2)*2 + *(s_x+m_width*2+2));

            int g;
            // Try using this instead of sqrt_table to see performance penalty
            // g = (int)qSqrt(gx*gx+gy*gy);
            // g = (g > 255 ? 255 : g);

            if (gx > 255 || gy > 255)
                g = 255;
            else
                g = sqrt_table[gx][gy];

            *d_x = g;
            *(d_x+1) = g;
            *(d_x+2) = g;
            *(d_x+3) = 0;

            d_x+=4;
            s_x+=2;
        }
    }
}
