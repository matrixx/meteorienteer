#include <QApplication>
#include "qmlapplicationviewer.h"
#include "taivaanvahti.h"

Q_DECL_EXPORT int main(int argc, char *argv[])
{
    QScopedPointer<QApplication> app(createApplication(argc, argv));

    QmlApplicationViewer viewer;
    viewer.setOrientation(QmlApplicationViewer::ScreenOrientationAuto);
    viewer.setMainQmlFile(QLatin1String("qml/Meteorienteer/main.qml"));
    viewer.showExpanded();
    Taivaanvahti tv;
    tv.getForm(1);
    return app->exec();
}
