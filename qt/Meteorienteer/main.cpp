#include <QApplication>
#include "qmlapplicationviewer.h"
#include "taivaanvahtitest.h"
#include <QDeclarativeContext>
#include <QDeclarativeEngine>
#include "taivaanvahti.h"
#include "taivaanvahtifield.h"
#include <QtDeclarative>

Q_DECL_EXPORT int main(int argc, char *argv[])
{
    QScopedPointer<QApplication> app(createApplication(argc, argv));

    QmlApplicationViewer viewer;
    viewer.setOrientation(QmlApplicationViewer::ScreenOrientationAuto);
    Taivaanvahti tv;
    viewer.rootContext()->setContextProperty("taivaanvahti", &tv);
    qmlRegisterType<TaivaanvahtiField>("tv", 1, 0, "TaivaanvahtiField");
    viewer.setMainQmlFile(QLatin1String("qml/Meteorienteer/main.qml"));
    viewer.showExpanded();
    TaivaanvahtiTest tvt;
    tvt.runTest();
    return app->exec();
}
