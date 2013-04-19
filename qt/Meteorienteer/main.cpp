#include <QApplication>
#include "qmlapplicationviewer.h"
#include "taivaanvahtitest.h"

Q_DECL_EXPORT int main(int argc, char *argv[])
{
    QScopedPointer<QApplication> app(createApplication(argc, argv));

    QmlApplicationViewer viewer;
    viewer.setOrientation(QmlApplicationViewer::ScreenOrientationAuto);
    viewer.setMainQmlFile(QLatin1String("qml/Meteorienteer/main.qml"));
    viewer.showExpanded();
    TaivaanvahtiTest tvt;
    tvt.runTest();
    return app->exec();
}
