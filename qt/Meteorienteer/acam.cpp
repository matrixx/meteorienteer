#include "acam.h"

ACam::ACam(QDeclarativeItem *parent) :
    QDeclarativeItem(parent)
{
    /*m_videoSurface = new AVideoSurface(NULL, )
    m_camera = new QCamera();
    m_camera->setCaptureMode(QCamera::CaptureStillImage);
    connect(m_camera, SIGNAL(error(QCamera::Error)), this, SLOT(error(QCamera::Error)));

    // Own video output drawing that shows camera view finder pictures
    QMediaService* ms = m_camera->service();
    QVideoRendererControl* vrc = ms->requestControl<QVideoRendererControl*>();
    m_myVideoSurface = new MyVideoSurface(this,this,this);
    vrc->setSurface(m_myVideoSurface);

    // Start camera
    if (m_camera->state() == QCamera::ActiveState) {
        m_camera->stop();
    }
//    m_videoWidget->show();
    m_camera->start();*/
}

void ACam::paint(QPainter *painter,
                 const QStyleOptionGraphicsItem *,
                 QWidget *widget )
{
    m_videoSurface->setWidget(widget);
    m_videoSurface->paint(painter);
    //
}
