#include <QApplication>
#include "qmlapplicationviewer.h"
#include "taivaanvahtitest.h"
#include <QDeclarativeContext>
#include <QDeclarativeEngine>
#include "formmanager.h"
#include <QtDeclarative>
#include <QSplashScreen>

Q_DECL_EXPORT int main(int argc, char *argv[])
{   
    QScopedPointer<QApplication> app(createApplication(argc, argv));
    QPixmap pixmap(":splash");
    QSplashScreen splash(pixmap);
    splash.setMask(pixmap.mask());
    splash.showFullScreen();
    QmlApplicationViewer viewer;
    viewer.setOrientation(QmlApplicationViewer::ScreenOrientationLockLandscape);
    QCoreApplication::setApplicationName("Meteorienteer");
    QCoreApplication::setOrganizationName("Meteorienteers");
    FormManager mgr;
    viewer.rootContext()->setContextProperty("mgr", &mgr);
    viewer.setMainQmlFile(QLatin1String("qml/Meteorienteer/main.qml"));
    //viewer.showExpanded();
    viewer.showFullScreen();
    splash.finish(&viewer);
//    TaivaanvahtiTest tvt;
//    tvt.runTest();
    return app->exec();
}
