#include <QApplication>
#include "qmlapplicationviewer.h"
#include "taivaanvahtitest.h"
#include <QDeclarativeContext>
#include <QDeclarativeEngine>
#include "formmanager.h"
#include <QtDeclarative>

Q_DECL_EXPORT int main(int argc, char *argv[])
{
    QScopedPointer<QApplication> app(createApplication(argc, argv));

    QmlApplicationViewer viewer;
    viewer.setOrientation(QmlApplicationViewer::ScreenOrientationLockLandscape);
    QCoreApplication::setApplicationName("Meteorienteer");
    QCoreApplication::setOrganizationName("Meteorienteers");
    FormManager mgr;
    viewer.rootContext()->setContextProperty("mgr", &mgr);
    viewer.setMainQmlFile(QLatin1String("qml/Meteorienteer/main.qml"));
    //viewer.showExpanded();
    viewer.showFullScreen();
//    TaivaanvahtiTest tvt;
//    tvt.runTest();
    return app->exec();
}
