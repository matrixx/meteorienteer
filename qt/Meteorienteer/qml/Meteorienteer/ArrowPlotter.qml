// import QtQuick 1.0 // to target S60 5th Edition or Maemo 5
import QtQuick 1.1

Rectangle {
    anchors.fill: parent;
    signal directionSelected(int direction);
    property int direction: 0;
    Image {
        id: arrow
        x:0; y:0;width:50;height:10;
        fillMode: Image.Stretch;
        transformOrigin: "Left"; smooth: false;
        source: "qrc:/gfx/arrow2.gif"
        z:10;
        visible:true;
        opacity: 50
    }
    Image {
        MouseArea {
            id: mouse_area1
            anchors.fill: parent
            property int mxi;
            property int myi;
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
                } else arrow.visible=false;
            }
        }
        anchors.fill: parent
        id: name
        source: "http://farm9.staticflickr.com/8251/8507360785_5fde045b8c_c.jpg"
    }
    function movedelta( mxi, myi, mxj, myj) {
        return Math.sqrt( Math.pow(mxj-mxi,2) + Math.pow(myj-myi,2) );
    }
}
