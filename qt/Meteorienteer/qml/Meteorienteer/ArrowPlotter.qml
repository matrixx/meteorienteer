// import QtQuick 1.0 // to target S60 5th Edition or Maemo 5
import QtQuick 1.1

Rectangle {
    id: arrowPlotter
    anchors.fill: parent;
    signal directionSelected(int direction);
    property int direction: 0;
    property alias imageUrl: capturedImage.source;
    Image {
        state: "normal"
        //visible: obview.shooterEnabled
        anchors.right: arrowPlotter.right
        anchors.verticalCenter: parent.verticalCenter
        anchors.rightMargin: 20
        width: 100;
        height: 100;
        z:30
        id: arrowOk
        Text {
            anchors.fill: parent
            wrapMode: Text.Wrap
            verticalAlignment: Text.AlignVCenter
            horizontalAlignment: Text.AlignHCenter
            id: arrowOkText
            color: "#ffffff"
            text: qsTr("Ok!")
            font.family: "Sans Serif"
            font.pointSize: 25
        }
        MouseArea {
            anchors.fill: parent
         onPressed: {
             arrowOk.state = "active"
             directionSelected(arrow.rotation+90);
         }
         onReleased: {
             shootButton.state = "normal"
         }
        }
        states: [
            State {
                name: "normal"
                PropertyChanges {
                    target: arrowOk
                    source: "qrc:/gfx/buttonNormal.png"
                }
            },
            State {
                name: "hover"
                PropertyChanges {
                    target: arrowOk
                    source: "qrc:/gfx/buttonHover.png"
                }
            },
            State {
                name: "active"
                PropertyChanges {
                    target: arrowOk
                    source: "qrc:/gfx/buttonActive.png"
                }
            }
        ]
    }
    Image {
        id: arrow
        x:300; y:300; width:200;height:80;
        fillMode: Image.Stretch;
        transformOrigin: "Left"; smooth: false;
        source: "qrc:/gfx/arrow1.gif"
        z:5;
        visible:true;
        opacity: 0.5;
        property int mxi;
        property int myi;
        property bool endGrabVisible: false
        Rectangle { anchors.rightMargin: width/-2; anchors.bottom: arrow.bottom; anchors.right: arrow.right; color: "#000010ff"; radius: 30; border.color: "darkgrey"; height: 80; width: 80;
            MouseArea {
                anchors.fill: parent;
                onPressed: {
                    console.debug( "mouse("+mxi+","+myi+") rg pressed");
                }
                onMousePositionChanged:{
                    var mouseXj = mapToItem(capturedImage,mouseX, mouseY).x
                    var mouseYj = mapToItem(capturedImage,mouseX, mouseY).y;
                    console.debug( "mouse("+mouseXj+","+mouseYj+") right grab position changed");
                    arrow.visible=true;
                    arrow.transformOrigin = "left";
                    arrow.rotation = Math.atan2(mouseYj-arrow.y,mouseXj-arrow.x)*180/(Math.PI);
                    arrow.width = movedelta(arrow.x,arrow.y,mouseXj,mouseYj);
                    console.debug( "arrowrg  width:" + arrow.width);
                }
            }
        }
        Rectangle { anchors.leftMargin: width/-2; anchors.bottom: arrow.bottom; anchors.left:arrow.left; color: "#000000ff"; radius: 3; border.color: "#f3aeae"; height: 80; width: 80;
            MouseArea {
                anchors.fill: parent;
                drag.axis: Drag.XandYAxis
                drag.target: arrow
            }
        }
    }
    Image {
        id: capturedImage;


    MouseArea {
        id: mouse_area1
        anchors.fill: parent
        property alias mxi: arrow.x
        property alias myi: arrow.y
        property int trigdelta: 20;

        onPressed: {
            myi=mouseY;
            mxi=mouseX;
            console.debug( "mouse("+mxi+","+myi+") pressed");
        }
        onReleased: {
            console.debug( "mouse("+mouseX+","+mouseY+") released");
            console.debug( "mouse move delta:" + movedelta(mxi, myi, mouseX, mouseY) );
            arrow.visible=true;
        }
        onMousePositionChanged: {
            if(movedelta(mxi,myi, mouseX, mouseY)>trigdelta && pressed) {
                arrow.x=mxi; arrow.y=myi;
                arrow.visible=true;
                arrow.x = mxi; arrow.y = myi;
                arrow.transformOrigin = "left";
                arrow.rotation = Math.atan2(mouseY-myi,mouseX-mxi)*180/(Math.PI);
                arrow.width = movedelta(mxi,myi,mouseX,mouseY);
                arrow.endGrabVisible = true;
            } else arrow.visible=false;
        }
    }
        anchors.fill: parent
        source: "qrc:/gfx/arrow2.gif"
    }
    function movedelta( mxi, myi, mxj, myj) {
        return Math.sqrt( Math.pow(mxj-mxi,2) + Math.pow(myj-myi,2) );
    }
}
