#ifndef MYVIDEOSURFACE_H
#define MYVIDEOSURFACE_H

#include <QAbstractVideoSurface>
#include <QVideoRendererControl>
#include <QVideoSurfaceFormat>
#include <QWidget>
#include <QPainter>
#include "uyvy2rgb.h"

/*****************************************************************************
* AVideoSurface
*/
class VideoIF
{
public:
    virtual void updateVideo() = 0;
};

<<<<<<< HEAD
class AVideoSurface: public QAbstractVideoSurface
=======
class MyVideoSurface: public QAbstractVideoSurface
>>>>>>> d7272e542ccdfb0679b90f5fa3afe395900eeb1a
{
Q_OBJECT

public:
    AVideoSurface(QWidget* widget, VideoIF* target, QObject * parent = 0);
    ~AVideoSurface();

    bool start(const QVideoSurfaceFormat &format);

    bool present(const QVideoFrame &frame);

    QList<QVideoFrame::PixelFormat> supportedPixelFormats(
                QAbstractVideoBuffer::HandleType handleType = QAbstractVideoBuffer::NoHandle) const;

    void paint(QPainter*);

    void calcLuma(uchar* luma, uchar* rgb, int width);
    void edgify(QImage* image);
    void setWidget(QWidget* w) { m_targetWidget = w; }

private:
    QWidget* m_targetWidget;
    VideoIF* m_target;
    QVideoFrame m_frame;
    QImage::Format m_imageFormat;
    int lumaR[256], lumaG[256], lumaB[256];
    int sqrt_table[256][256];

    UYVY2RGB uyvy2rgb;
};

#endif // MYVIDEOSURFACE_H
