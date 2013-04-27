// import QtQuick 1.0 // to target S60 5th Edition or Maemo 5
import QtQuick 1.1

Rectangle {
    id: arrowPlotter
    anchors.fill: parent;
    signal directionSelected(int direction);
    property int direction: 0;
    property alias imageUrl: capturedImage.source;
    Image {
        id: arrowOk
        x:0; y:0;width:500;height:100;
        source: "qrc:/gfx/buttonNormal.png"
        Text {
            anchors.left: parent.left
            anchors.top: parent.top
            id: arrowText
            text: qsTr("Draw flight direction and press ok")
        }
         z:10;
        MouseArea {
            anchors.fill: parent;
            onPressed: {
                console.log("send arrow rotation" + arrow.rotation+90);
                directionSelected(arrow.rotation+90);
            }
        }
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
        Rectangle { anchors.rightMargin: width/-2; anchors.bottom: arrow.bottom; anchors.right: arrow.right; color: "red"; height: 80; width: 80;
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
        Rectangle { anchors.leftMargin: width/-2; anchors.bottom: arrow.bottom; anchors.left:arrow.left; color: "red"; height: 80; width: 80;
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
