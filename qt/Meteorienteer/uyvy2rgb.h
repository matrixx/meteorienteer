#ifndef UYVY2RGB_H
#define UYVY2RGB_H

#include <QVideoFrame>

class UYVY2RGB : public QObject
{
    Q_OBJECT

    public:
        UYVY2RGB();
        ~UYVY2RGB();

    public:
        void initData(const QVideoFrame &source);

        unsigned char* bits();
        int width();
        int height();

    private:
        int sqrt_table[256][256];

        unsigned char *m_bits;
        int m_width;
        int m_height;
};

#endif // UYVY2RGB_H
