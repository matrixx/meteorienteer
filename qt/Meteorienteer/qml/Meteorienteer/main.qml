import QtQuick 1.1
import com.nokia.meego 1.0

PageStackWindow {
    id: appWindow
    initialPage: mainPage
    Component.onCompleted: {
        theme.inverted = true
    }

    MainPage {
        id: mainPage
    }

    ToolBarLayout {
        id: commonTools

        ToolIcon {
            visible: pageStack.depth > 1
            platformIconId: "toolbar-back"
            onClicked: {
                pageStack.pop();
            }
        }

        ToolIcon {
            platformIconId: "toolbar-view-menu"
            onClicked: {
                appWindow.showToolBar = false;
                myMenu.open();
            }
        }
    }

    QueryDialog {
        id: aboutDialog
        titleText: "Meteorienteer"
        message: "(C) [2013] [Meteorienteers]\n[0.1]"
    }

    Menu {
        id: myMenu
        visualParent: pageStack
        MenuLayout {
            MenuItem {
                text: qsTr("About")
                onClicked: {
                    aboutDialog.open();
                }
            }
        }
        onStatusChanged: {
            if (status === DialogStatus.Closed) {
                appWindow.showToolBar = true;
            }
        }
    }
}
