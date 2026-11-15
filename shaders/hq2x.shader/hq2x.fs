/*
 * This file is part of the Advance project.
 *
 * Copyright (C) 2003 Andrea Mazzoleni
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software Foundation,
 * Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 *
 * In addition, as a special exception, Andrea Mazzoleni
 * gives permission to link the code of this program with
 * the MAME library (or with modified versions of MAME that use the
 * same license as MAME), and distribute linked combinations including
 * the two.  You must obey the GNU General Public License in all
 * respects for all of the code used other than MAME.  If you modify
 * this file, you may extend this exception to your version of the
 * file, but you are not obligated to do so.  If you do not wish to
 * do so, delete this exception statement from your version.
 */

/*
    VisualBoyAdvance - a Game Boy & Game Boy Advance emulator

    Copyright (C) 2008 VBA-M development team


    This program is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 2 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA

        hq filter by Maxim Stepin ( http://hiend3d.com )
*/


// Direct port of VBA-M portable 32-bit HQX tables. GPL-2.0-or-later.

varying vec2 texCoord;
uniform sampler2D tex;
uniform vec2 texSize;
bool diff2(ivec3 a, ivec3 b) {
 if(all(equal(a/8,b/8))) return false;
 ivec3 d=a-b;
 return abs(d.r+d.g+d.b)>192 || abs(d.r-d.b)>28 || abs(-d.r+2*d.g-d.b)>48;
}
int yuv(ivec3 c) {return ((c.r+c.g+c.b)<<14)+((c.r-c.b+512)<<4)+((2*c.g-c.r-c.b)>>3)+128;}
bool Diff(int a,int b) {
 return (((a&0xff0000)-(b&0xff0000))&0x7fffffff)>0x300000 ||
 (((a&0xff00)-(b&0xff00))&0x7fffffff)>0x700 ||
 (((a&0xff)-(b&0xff))&0x7fffffff)>6;
}
ivec3 calculate(ivec3 c[10],int yy[10],int pixel,int pattern){
if(pattern<128){
if(pattern<64){
if(pattern<32){
if(pattern<16){
if(pattern<8){
if(pattern<4){
if(pattern<2){
if(pattern<1){

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}

}else{
if(pattern<3){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}

}

}else{
if(pattern<6){
if(pattern<5){

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}

}else{
if(pattern<7){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}

}

}

}else{
if(pattern<12){
if(pattern<10){
if(pattern<9){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}

}else{
if(pattern<11){

        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        } else {
                { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        }

}else{

        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return c[4]; }
        } else {
                { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        }

}

}

}else{
if(pattern<14){
if(pattern<13){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}

}else{
if(pattern<15){

        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
                { if(pixel==1) return (c[4]*3+c[5]*1)/4; }
        } else {
                { if(pixel==0) return (c[1]*3+c[3]*3+c[4]*2)/8; }
                { if(pixel==1) return (c[4]*5+c[1]*2+c[5]*1)/8; }
        }

}else{

        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return c[4]; }
                { if(pixel==1) return (c[4]*3+c[5]*1)/4; }
        } else {
                { if(pixel==0) return (c[1]*3+c[3]*3+c[4]*2)/8; }
                { if(pixel==1) return (c[4]*5+c[1]*2+c[5]*1)/8; }
        }

}

}

}

}

}else{
if(pattern<24){
if(pattern<20){
if(pattern<18){
if(pattern<17){

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }

}

}else{
if(pattern<19){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        } else {
                { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        }

}else{

        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        if (diff2(c[1],c[5])) {
                { if(pixel==0) return (c[4]*3+c[3]*1)/4; }
                { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        } else {
                { if(pixel==0) return (c[4]*5+c[1]*2+c[3]*1)/8; }
                { if(pixel==1) return (c[1]*3+c[5]*3+c[4]*2)/8; }
        }

}

}

}else{
if(pattern<22){
if(pattern<21){

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }

}

}else{
if(pattern<23){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return c[4]; }
        } else {
                { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        }

}else{

        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        if (diff2(c[1],c[5])) {
                { if(pixel==0) return (c[4]*3+c[3]*1)/4; }
                { if(pixel==1) return c[4]; }
        } else {
                { if(pixel==0) return (c[4]*5+c[1]*2+c[3]*1)/8; }
                { if(pixel==1) return (c[1]*3+c[5]*3+c[4]*2)/8; }
        }

}

}

}

}else{
if(pattern<28){
if(pattern<26){
if(pattern<25){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }

}

}else{
if(pattern<27){

        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return c[4]; }
        } else {
                { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return c[4]; }
        } else {
                { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        }

}else{

        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return c[4]; }
        } else {
                { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        }

}

}

}else{
if(pattern<30){
if(pattern<29){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }

}

}else{
if(pattern<31){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return c[4]; }
        } else {
                { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        }

}else{

        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return c[4]; }
        } else {
                { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return c[4]; }
        } else {
                { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        }

}

}

}

}

}

}else{
if(pattern<48){
if(pattern<40){
if(pattern<36){
if(pattern<34){
if(pattern<33){

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}

}else{
if(pattern<35){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}

}

}else{
if(pattern<38){
if(pattern<37){

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}

}else{
if(pattern<39){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}

}

}

}else{
if(pattern<44){
if(pattern<42){
if(pattern<41){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}

}else{
if(pattern<43){

        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
                { if(pixel==2) return (c[4]*3+c[7]*1)/4; }
        } else {
                { if(pixel==0) return (c[1]*3+c[3]*3+c[4]*2)/8; }
                { if(pixel==2) return (c[4]*5+c[3]*2+c[7]*1)/8; }
        }

}else{

        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return c[4]; }
                { if(pixel==2) return (c[4]*3+c[7]*1)/4; }
        } else {
                { if(pixel==0) return (c[1]*3+c[3]*3+c[4]*2)/8; }
                { if(pixel==2) return (c[4]*5+c[3]*2+c[7]*1)/8; }
        }

}

}

}else{
if(pattern<46){
if(pattern<45){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}

}else{
if(pattern<47){

        { if(pixel==1) return (c[4]*3+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        } else {
                { if(pixel==0) return (c[4]*6+c[1]*1+c[3]*1)/8; }
        }

}else{

        { if(pixel==1) return (c[4]*3+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return c[4]; }
        } else {
                { if(pixel==0) return (c[4]*14+c[1]*1+c[3]*1)/16; }
        }

}

}

}

}

}else{
if(pattern<56){
if(pattern<52){
if(pattern<50){
if(pattern<49){

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }

}

}else{
if(pattern<51){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        } else {
                { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        }

}else{

        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        if (diff2(c[1],c[5])) {
                { if(pixel==0) return (c[4]*3+c[3]*1)/4; }
                { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        } else {
                { if(pixel==0) return (c[4]*5+c[1]*2+c[3]*1)/8; }
                { if(pixel==1) return (c[1]*3+c[5]*3+c[4]*2)/8; }
        }

}

}

}else{
if(pattern<54){
if(pattern<53){

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }

}

}else{
if(pattern<55){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return c[4]; }
        } else {
                { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        }

}else{

        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        if (diff2(c[1],c[5])) {
                { if(pixel==0) return (c[4]*3+c[3]*1)/4; }
                { if(pixel==1) return c[4]; }
        } else {
                { if(pixel==0) return (c[4]*5+c[1]*2+c[3]*1)/8; }
                { if(pixel==1) return (c[1]*3+c[5]*3+c[4]*2)/8; }
        }

}

}

}

}else{
if(pattern<60){
if(pattern<58){
if(pattern<57){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }

}

}else{
if(pattern<59){

        { if(pixel==2) return (c[4]*3+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        } else {
                { if(pixel==0) return (c[4]*6+c[1]*1+c[3]*1)/8; }
        }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        } else {
                { if(pixel==1) return (c[4]*6+c[1]*1+c[5]*1)/8; }
        }

}else{

        { if(pixel==2) return (c[4]*3+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return c[4]; }
        } else {
                { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        } else {
                { if(pixel==1) return (c[4]*6+c[1]*1+c[5]*1)/8; }
        }

}

}

}else{
if(pattern<62){
if(pattern<61){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }

}

}else{
if(pattern<63){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return c[4]; }
        } else {
                { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        }

}else{

        { if(pixel==2) return (c[4]*3+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return c[4]; }
        } else {
                { if(pixel==0) return (c[4]*14+c[1]*1+c[3]*1)/16; }
        }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return c[4]; }
        } else {
                { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        }

}

}

}

}

}

}

}else{
if(pattern<96){
if(pattern<80){
if(pattern<72){
if(pattern<68){
if(pattern<66){
if(pattern<65){

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }

}

}else{
if(pattern<67){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }

}

}

}else{
if(pattern<70){
if(pattern<69){

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }

}

}else{
if(pattern<71){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }

}

}

}

}else{
if(pattern<76){
if(pattern<74){
if(pattern<73){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        } else {
                { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        }

}else{

        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==0) return (c[4]*3+c[1]*1)/4; }
                { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        } else {
                { if(pixel==0) return (c[4]*5+c[3]*2+c[1]*1)/8; }
                { if(pixel==2) return (c[3]*3+c[7]*3+c[4]*2)/8; }
        }

}

}else{
if(pattern<75){

        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return c[4]; }
        } else {
                { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return c[4]; }
        } else {
                { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        }

}else{

        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return c[4]; }
        } else {
                { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        }

}

}

}else{
if(pattern<78){
if(pattern<77){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        } else {
                { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        }

}else{

        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==0) return (c[4]*3+c[1]*1)/4; }
                { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        } else {
                { if(pixel==0) return (c[4]*5+c[3]*2+c[1]*1)/8; }
                { if(pixel==2) return (c[3]*3+c[7]*3+c[4]*2)/8; }
        }

}

}else{
if(pattern<79){

        { if(pixel==1) return (c[4]*3+c[5]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        } else {
                { if(pixel==2) return (c[4]*6+c[3]*1+c[7]*1)/8; }
        }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        } else {
                { if(pixel==0) return (c[4]*6+c[1]*1+c[3]*1)/8; }
        }

}else{

        { if(pixel==1) return (c[4]*3+c[5]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        } else {
                { if(pixel==2) return (c[4]*6+c[3]*1+c[7]*1)/8; }
        }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return c[4]; }
        } else {
                { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        }

}

}

}

}

}else{
if(pattern<88){
if(pattern<84){
if(pattern<82){
if(pattern<81){

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        } else {
                { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }
        }

}else{

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        } else {
                { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }
        }

}

}else{
if(pattern<83){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return c[4]; }
        } else {
                { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }
        }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return c[4]; }
        } else {
                { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        }

}else{

        { if(pixel==0) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        } else {
                { if(pixel==3) return (c[4]*6+c[5]*1+c[7]*1)/8; }
        }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        } else {
                { if(pixel==1) return (c[4]*6+c[1]*1+c[5]*1)/8; }
        }

}

}

}else{
if(pattern<86){
if(pattern<85){

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        if (diff2(c[5],c[7])) {
                { if(pixel==1) return (c[4]*3+c[1]*1)/4; }
                { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        } else {
                { if(pixel==1) return (c[4]*5+c[5]*2+c[1]*1)/8; }
                { if(pixel==3) return (c[5]*3+c[7]*3+c[4]*2)/8; }
        }

}else{

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        if (diff2(c[5],c[7])) {
                { if(pixel==1) return (c[4]*3+c[1]*1)/4; }
                { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        } else {
                { if(pixel==1) return (c[4]*5+c[5]*2+c[1]*1)/8; }
                { if(pixel==3) return (c[5]*3+c[7]*3+c[4]*2)/8; }
        }

}

}else{
if(pattern<87){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return c[4]; }
        } else {
                { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        }

}else{

        { if(pixel==0) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        } else {
                { if(pixel==3) return (c[4]*6+c[5]*1+c[7]*1)/8; }
        }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return c[4]; }
        } else {
                { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        }

}

}

}

}else{
if(pattern<92){
if(pattern<90){
if(pattern<89){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return c[4]; }
        } else {
                { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return c[4]; }
        } else {
                { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }
        }

}else{

        { if(pixel==0) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        } else {
                { if(pixel==2) return (c[4]*6+c[3]*1+c[7]*1)/8; }
        }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        } else {
                { if(pixel==3) return (c[4]*6+c[5]*1+c[7]*1)/8; }
        }

}

}else{
if(pattern<91){

        if (diff2(c[7],c[3])) {
                { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        } else {
                { if(pixel==2) return (c[4]*6+c[3]*1+c[7]*1)/8; }
        }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        } else {
                { if(pixel==3) return (c[4]*6+c[5]*1+c[7]*1)/8; }
        }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        } else {
                { if(pixel==0) return (c[4]*6+c[1]*1+c[3]*1)/8; }
        }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        } else {
                { if(pixel==1) return (c[4]*6+c[1]*1+c[5]*1)/8; }
        }

}else{

        if (diff2(c[7],c[3])) {
                { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        } else {
                { if(pixel==2) return (c[4]*6+c[3]*1+c[7]*1)/8; }
        }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        } else {
                { if(pixel==3) return (c[4]*6+c[5]*1+c[7]*1)/8; }
        }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return c[4]; }
        } else {
                { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        } else {
                { if(pixel==1) return (c[4]*6+c[1]*1+c[5]*1)/8; }
        }

}

}

}else{
if(pattern<94){
if(pattern<93){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[1]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        } else {
                { if(pixel==2) return (c[4]*6+c[3]*1+c[7]*1)/8; }
        }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        } else {
                { if(pixel==3) return (c[4]*6+c[5]*1+c[7]*1)/8; }
        }

}else{

        { if(pixel==0) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[1]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        } else {
                { if(pixel==2) return (c[4]*6+c[3]*1+c[7]*1)/8; }
        }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        } else {
                { if(pixel==3) return (c[4]*6+c[5]*1+c[7]*1)/8; }
        }

}

}else{
if(pattern<95){

        if (diff2(c[7],c[3])) {
                { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        } else {
                { if(pixel==2) return (c[4]*6+c[3]*1+c[7]*1)/8; }
        }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        } else {
                { if(pixel==3) return (c[4]*6+c[5]*1+c[7]*1)/8; }
        }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        } else {
                { if(pixel==0) return (c[4]*6+c[1]*1+c[3]*1)/8; }
        }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return c[4]; }
        } else {
                { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        }

}else{

        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return c[4]; }
        } else {
                { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return c[4]; }
        } else {
                { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        }

}

}

}

}

}

}else{
if(pattern<112){
if(pattern<104){
if(pattern<100){
if(pattern<98){
if(pattern<97){

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }

}

}else{
if(pattern<99){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }

}

}

}else{
if(pattern<102){
if(pattern<101){

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }

}

}else{
if(pattern<103){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }

}

}

}

}else{
if(pattern<108){
if(pattern<106){
if(pattern<105){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return c[4]; }
        } else {
                { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        }

}else{

        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==0) return (c[4]*3+c[1]*1)/4; }
                { if(pixel==2) return c[4]; }
        } else {
                { if(pixel==0) return (c[4]*5+c[3]*2+c[1]*1)/8; }
                { if(pixel==2) return (c[3]*3+c[7]*3+c[4]*2)/8; }
        }

}

}else{
if(pattern<107){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return c[4]; }
        } else {
                { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        }

}else{

        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return c[4]; }
        } else {
                { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return c[4]; }
        } else {
                { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        }

}

}

}else{
if(pattern<110){
if(pattern<109){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return c[4]; }
        } else {
                { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        }

}else{

        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==0) return (c[4]*3+c[1]*1)/4; }
                { if(pixel==2) return c[4]; }
        } else {
                { if(pixel==0) return (c[4]*5+c[3]*2+c[1]*1)/8; }
                { if(pixel==2) return (c[3]*3+c[7]*3+c[4]*2)/8; }
        }

}

}else{
if(pattern<111){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[5]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return c[4]; }
        } else {
                { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        }

}else{

        { if(pixel==1) return (c[4]*3+c[5]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return c[4]; }
        } else {
                { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return c[4]; }
        } else {
                { if(pixel==0) return (c[4]*14+c[1]*1+c[3]*1)/16; }
        }

}

}

}

}

}else{
if(pattern<120){
if(pattern<116){
if(pattern<114){
if(pattern<113){

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        if (diff2(c[5],c[7])) {
                { if(pixel==2) return (c[4]*3+c[3]*1)/4; }
                { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        } else {
                { if(pixel==2) return (c[4]*5+c[7]*2+c[3]*1)/8; }
                { if(pixel==3) return (c[5]*3+c[7]*3+c[4]*2)/8; }
        }

}else{

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        if (diff2(c[5],c[7])) {
                { if(pixel==2) return (c[4]*3+c[3]*1)/4; }
                { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        } else {
                { if(pixel==2) return (c[4]*5+c[7]*2+c[3]*1)/8; }
                { if(pixel==3) return (c[5]*3+c[7]*3+c[4]*2)/8; }
        }

}

}else{
if(pattern<115){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[3]*1)/4; }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        } else {
                { if(pixel==3) return (c[4]*6+c[5]*1+c[7]*1)/8; }
        }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        } else {
                { if(pixel==1) return (c[4]*6+c[1]*1+c[5]*1)/8; }
        }

}else{

        { if(pixel==0) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[3]*1)/4; }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        } else {
                { if(pixel==3) return (c[4]*6+c[5]*1+c[7]*1)/8; }
        }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        } else {
                { if(pixel==1) return (c[4]*6+c[1]*1+c[5]*1)/8; }
        }

}

}

}else{
if(pattern<118){
if(pattern<117){

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[3]*1)/4; }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        } else {
                { if(pixel==3) return (c[4]*6+c[5]*1+c[7]*1)/8; }
        }

}else{

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[3]*1)/4; }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        } else {
                { if(pixel==3) return (c[4]*6+c[5]*1+c[7]*1)/8; }
        }

}

}else{
if(pattern<119){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return c[4]; }
        } else {
                { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        }

}else{

        { if(pixel==2) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        if (diff2(c[1],c[5])) {
                { if(pixel==0) return (c[4]*3+c[3]*1)/4; }
                { if(pixel==1) return c[4]; }
        } else {
                { if(pixel==0) return (c[4]*5+c[1]*2+c[3]*1)/8; }
                { if(pixel==1) return (c[1]*3+c[5]*3+c[4]*2)/8; }
        }

}

}

}

}else{
if(pattern<124){
if(pattern<122){
if(pattern<121){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return c[4]; }
        } else {
                { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        }

}else{

        { if(pixel==0) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return c[4]; }
        } else {
                { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        } else {
                { if(pixel==3) return (c[4]*6+c[5]*1+c[7]*1)/8; }
        }

}

}else{
if(pattern<123){

        if (diff2(c[7],c[3])) {
                { if(pixel==2) return c[4]; }
        } else {
                { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        } else {
                { if(pixel==3) return (c[4]*6+c[5]*1+c[7]*1)/8; }
        }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        } else {
                { if(pixel==0) return (c[4]*6+c[1]*1+c[3]*1)/8; }
        }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        } else {
                { if(pixel==1) return (c[4]*6+c[1]*1+c[5]*1)/8; }
        }

}else{

        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return c[4]; }
        } else {
                { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return c[4]; }
        } else {
                { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        }

}

}

}else{
if(pattern<126){
if(pattern<125){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return c[4]; }
        } else {
                { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        }

}else{

        { if(pixel==1) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==0) return (c[4]*3+c[1]*1)/4; }
                { if(pixel==2) return c[4]; }
        } else {
                { if(pixel==0) return (c[4]*5+c[3]*2+c[1]*1)/8; }
                { if(pixel==2) return (c[3]*3+c[7]*3+c[4]*2)/8; }
        }

}

}else{
if(pattern<127){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return c[4]; }
        } else {
                { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return c[4]; }
        } else {
                { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        }

}else{

        { if(pixel==3) return (c[4]*3+c[8]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return c[4]; }
        } else {
                { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return c[4]; }
        } else {
                { if(pixel==0) return (c[4]*14+c[1]*1+c[3]*1)/16; }
        }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return c[4]; }
        } else {
                { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        }

}

}

}

}

}

}

}

}else{
if(pattern<192){
if(pattern<160){
if(pattern<144){
if(pattern<136){
if(pattern<132){
if(pattern<130){
if(pattern<129){

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}

}else{
if(pattern<131){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}

}

}else{
if(pattern<134){
if(pattern<133){

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}

}else{
if(pattern<135){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}

}

}

}else{
if(pattern<140){
if(pattern<138){
if(pattern<137){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}

}else{
if(pattern<139){

        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        } else {
                { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        }

}else{

        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return c[4]; }
        } else {
                { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        }

}

}

}else{
if(pattern<142){
if(pattern<141){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}

}else{
if(pattern<143){

        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
                { if(pixel==1) return (c[4]*3+c[5]*1)/4; }
        } else {
                { if(pixel==0) return (c[1]*3+c[3]*3+c[4]*2)/8; }
                { if(pixel==1) return (c[4]*5+c[1]*2+c[5]*1)/8; }
        }

}else{

        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return c[4]; }
                { if(pixel==1) return (c[4]*3+c[5]*1)/4; }
        } else {
                { if(pixel==0) return (c[1]*3+c[3]*3+c[4]*2)/8; }
                { if(pixel==1) return (c[4]*5+c[1]*2+c[5]*1)/8; }
        }

}

}

}

}

}else{
if(pattern<152){
if(pattern<148){
if(pattern<146){
if(pattern<145){

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[7]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[7]*1)/4; }

}

}else{
if(pattern<147){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
                { if(pixel==3) return (c[4]*3+c[7]*1)/4; }
        } else {
                { if(pixel==1) return (c[1]*3+c[5]*3+c[4]*2)/8; }
                { if(pixel==3) return (c[4]*5+c[5]*2+c[7]*1)/8; }
        }

}else{

        { if(pixel==0) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[7]*1)/4; }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        } else {
                { if(pixel==1) return (c[4]*6+c[1]*1+c[5]*1)/8; }
        }

}

}

}else{
if(pattern<150){
if(pattern<149){

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[7]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[7]*1)/4; }

}

}else{
if(pattern<151){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return c[4]; }
                { if(pixel==3) return (c[4]*3+c[7]*1)/4; }
        } else {
                { if(pixel==1) return (c[1]*3+c[5]*3+c[4]*2)/8; }
                { if(pixel==3) return (c[4]*5+c[5]*2+c[7]*1)/8; }
        }

}else{

        { if(pixel==0) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[7]*1)/4; }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return c[4]; }
        } else {
                { if(pixel==1) return (c[4]*14+c[1]*1+c[5]*1)/16; }
        }

}

}

}

}else{
if(pattern<156){
if(pattern<154){
if(pattern<153){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[7]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[7]*1)/4; }

}

}else{
if(pattern<155){

        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[7]*1)/4; }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        } else {
                { if(pixel==0) return (c[4]*6+c[1]*1+c[3]*1)/8; }
        }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        } else {
                { if(pixel==1) return (c[4]*6+c[1]*1+c[5]*1)/8; }
        }

}else{

        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[7]*1)/4; }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return c[4]; }
        } else {
                { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        }

}

}

}else{
if(pattern<158){
if(pattern<157){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[7]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[7]*1)/4; }

}

}else{
if(pattern<159){

        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[7]*1)/4; }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        } else {
                { if(pixel==0) return (c[4]*6+c[1]*1+c[3]*1)/8; }
        }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return c[4]; }
        } else {
                { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        }

}else{

        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[7]*1)/4; }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return c[4]; }
        } else {
                { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return c[4]; }
        } else {
                { if(pixel==1) return (c[4]*14+c[1]*1+c[5]*1)/16; }
        }

}

}

}

}

}

}else{
if(pattern<176){
if(pattern<168){
if(pattern<164){
if(pattern<162){
if(pattern<161){

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}

}else{
if(pattern<163){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}

}

}else{
if(pattern<166){
if(pattern<165){

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}

}else{
if(pattern<167){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}

}

}

}else{
if(pattern<172){
if(pattern<170){
if(pattern<169){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}

}else{
if(pattern<171){

        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
                { if(pixel==2) return (c[4]*3+c[7]*1)/4; }
        } else {
                { if(pixel==0) return (c[1]*3+c[3]*3+c[4]*2)/8; }
                { if(pixel==2) return (c[4]*5+c[3]*2+c[7]*1)/8; }
        }

}else{

        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return c[4]; }
                { if(pixel==2) return (c[4]*3+c[7]*1)/4; }
        } else {
                { if(pixel==0) return (c[1]*3+c[3]*3+c[4]*2)/8; }
                { if(pixel==2) return (c[4]*5+c[3]*2+c[7]*1)/8; }
        }

}

}

}else{
if(pattern<174){
if(pattern<173){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }

}

}else{
if(pattern<175){

        { if(pixel==1) return (c[4]*3+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        } else {
                { if(pixel==0) return (c[4]*6+c[1]*1+c[3]*1)/8; }
        }

}else{

        { if(pixel==1) return (c[4]*3+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return c[4]; }
        } else {
                { if(pixel==0) return (c[4]*14+c[1]*1+c[3]*1)/16; }
        }

}

}

}

}

}else{
if(pattern<184){
if(pattern<180){
if(pattern<178){
if(pattern<177){

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[7]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[7]*1)/4; }

}

}else{
if(pattern<179){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
                { if(pixel==3) return (c[4]*3+c[7]*1)/4; }
        } else {
                { if(pixel==1) return (c[1]*3+c[5]*3+c[4]*2)/8; }
                { if(pixel==3) return (c[4]*5+c[5]*2+c[7]*1)/8; }
        }

}else{

        { if(pixel==0) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[7]*1)/4; }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        } else {
                { if(pixel==1) return (c[4]*6+c[1]*1+c[5]*1)/8; }
        }

}

}

}else{
if(pattern<182){
if(pattern<181){

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[7]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[7]*1)/4; }

}

}else{
if(pattern<183){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return c[4]; }
                { if(pixel==3) return (c[4]*3+c[7]*1)/4; }
        } else {
                { if(pixel==1) return (c[1]*3+c[5]*3+c[4]*2)/8; }
                { if(pixel==3) return (c[4]*5+c[5]*2+c[7]*1)/8; }
        }

}else{

        { if(pixel==0) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[7]*1)/4; }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return c[4]; }
        } else {
                { if(pixel==1) return (c[4]*14+c[1]*1+c[5]*1)/16; }
        }

}

}

}

}else{
if(pattern<188){
if(pattern<186){
if(pattern<185){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[7]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[7]*1)/4; }

}

}else{
if(pattern<187){

        { if(pixel==2) return (c[4]*3+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[7]*1)/4; }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        } else {
                { if(pixel==0) return (c[4]*6+c[1]*1+c[3]*1)/8; }
        }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        } else {
                { if(pixel==1) return (c[4]*6+c[1]*1+c[5]*1)/8; }
        }

}else{

        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[7]*1)/4; }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return c[4]; }
                { if(pixel==2) return (c[4]*3+c[7]*1)/4; }
        } else {
                { if(pixel==0) return (c[1]*3+c[3]*3+c[4]*2)/8; }
                { if(pixel==2) return (c[4]*5+c[3]*2+c[7]*1)/8; }
        }

}

}

}else{
if(pattern<190){
if(pattern<189){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[7]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[7]*1)/4; }

}

}else{
if(pattern<191){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[7]*1)/4; }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return c[4]; }
                { if(pixel==3) return (c[4]*3+c[7]*1)/4; }
        } else {
                { if(pixel==1) return (c[1]*3+c[5]*3+c[4]*2)/8; }
                { if(pixel==3) return (c[4]*5+c[5]*2+c[7]*1)/8; }
        }

}else{

        { if(pixel==2) return (c[4]*3+c[7]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[7]*1)/4; }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return c[4]; }
        } else {
                { if(pixel==0) return (c[4]*14+c[1]*1+c[3]*1)/16; }
        }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return c[4]; }
        } else {
                { if(pixel==1) return (c[4]*14+c[1]*1+c[5]*1)/16; }
        }

}

}

}

}

}

}

}else{
if(pattern<224){
if(pattern<208){
if(pattern<200){
if(pattern<196){
if(pattern<194){
if(pattern<193){

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[5]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[5]*1)/4; }

}

}else{
if(pattern<195){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[5]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[5]*1)/4; }

}

}

}else{
if(pattern<198){
if(pattern<197){

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[5]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[5]*1)/4; }

}

}else{
if(pattern<199){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[5]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[5]*1)/4; }

}

}

}

}else{
if(pattern<204){
if(pattern<202){
if(pattern<201){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
                { if(pixel==3) return (c[4]*3+c[5]*1)/4; }
        } else {
                { if(pixel==2) return (c[3]*3+c[7]*3+c[4]*2)/8; }
                { if(pixel==3) return (c[4]*5+c[7]*2+c[5]*1)/8; }
        }

}else{

        { if(pixel==0) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[5]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        } else {
                { if(pixel==2) return (c[4]*6+c[3]*1+c[7]*1)/8; }
        }

}

}else{
if(pattern<203){

        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[5]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        } else {
                { if(pixel==2) return (c[4]*6+c[3]*1+c[7]*1)/8; }
        }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        } else {
                { if(pixel==0) return (c[4]*6+c[1]*1+c[3]*1)/8; }
        }

}else{

        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[5]*1)/4; }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return c[4]; }
        } else {
                { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        }

}

}

}else{
if(pattern<206){
if(pattern<205){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
                { if(pixel==3) return (c[4]*3+c[5]*1)/4; }
        } else {
                { if(pixel==2) return (c[3]*3+c[7]*3+c[4]*2)/8; }
                { if(pixel==3) return (c[4]*5+c[7]*2+c[5]*1)/8; }
        }

}else{

        { if(pixel==0) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[5]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        } else {
                { if(pixel==2) return (c[4]*6+c[3]*1+c[7]*1)/8; }
        }

}

}else{
if(pattern<207){

        { if(pixel==1) return (c[4]*3+c[5]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[5]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        } else {
                { if(pixel==2) return (c[4]*6+c[3]*1+c[7]*1)/8; }
        }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        } else {
                { if(pixel==0) return (c[4]*6+c[1]*1+c[3]*1)/8; }
        }

}else{

        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[5]*1)/4; }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return c[4]; }
                { if(pixel==1) return (c[4]*3+c[5]*1)/4; }
        } else {
                { if(pixel==0) return (c[1]*3+c[3]*3+c[4]*2)/8; }
                { if(pixel==1) return (c[4]*5+c[1]*2+c[5]*1)/8; }
        }

}

}

}

}

}else{
if(pattern<216){
if(pattern<212){
if(pattern<210){
if(pattern<209){

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return c[4]; }
        } else {
                { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }
        }

}else{

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return c[4]; }
        } else {
                { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }
        }

}

}else{
if(pattern<211){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return c[4]; }
        } else {
                { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }
        }

}else{

        { if(pixel==0) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return c[4]; }
        } else {
                { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }
        }

}

}

}else{
if(pattern<214){
if(pattern<213){

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        if (diff2(c[5],c[7])) {
                { if(pixel==1) return (c[4]*3+c[1]*1)/4; }
                { if(pixel==3) return c[4]; }
        } else {
                { if(pixel==1) return (c[4]*5+c[5]*2+c[1]*1)/8; }
                { if(pixel==3) return (c[5]*3+c[7]*3+c[4]*2)/8; }
        }

}else{

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        if (diff2(c[5],c[7])) {
                { if(pixel==1) return (c[4]*3+c[1]*1)/4; }
                { if(pixel==3) return c[4]; }
        } else {
                { if(pixel==1) return (c[4]*5+c[5]*2+c[1]*1)/8; }
                { if(pixel==3) return (c[5]*3+c[7]*3+c[4]*2)/8; }
        }

}

}else{
if(pattern<215){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return c[4]; }
        } else {
                { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }
        }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return c[4]; }
        } else {
                { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        }

}else{

        { if(pixel==0) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return c[4]; }
        } else {
                { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }
        }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return c[4]; }
        } else {
                { if(pixel==1) return (c[4]*14+c[1]*1+c[5]*1)/16; }
        }

}

}

}

}else{
if(pattern<220){
if(pattern<218){
if(pattern<217){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return c[4]; }
        } else {
                { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }
        }

}else{

        { if(pixel==0) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return c[4]; }
        } else {
                { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }
        }

}

}else{
if(pattern<219){

        if (diff2(c[7],c[3])) {
                { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        } else {
                { if(pixel==2) return (c[4]*6+c[3]*1+c[7]*1)/8; }
        }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return c[4]; }
        } else {
                { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }
        }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        } else {
                { if(pixel==0) return (c[4]*6+c[1]*1+c[3]*1)/8; }
        }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        } else {
                { if(pixel==1) return (c[4]*6+c[1]*1+c[5]*1)/8; }
        }

}else{

        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return c[4]; }
        } else {
                { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }
        }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return c[4]; }
        } else {
                { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        }

}

}

}else{
if(pattern<222){
if(pattern<221){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[1]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        } else {
                { if(pixel==2) return (c[4]*6+c[3]*1+c[7]*1)/8; }
        }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return c[4]; }
        } else {
                { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }
        }

}else{

        { if(pixel==0) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        if (diff2(c[5],c[7])) {
                { if(pixel==1) return (c[4]*3+c[1]*1)/4; }
                { if(pixel==3) return c[4]; }
        } else {
                { if(pixel==1) return (c[4]*5+c[5]*2+c[1]*1)/8; }
                { if(pixel==3) return (c[5]*3+c[7]*3+c[4]*2)/8; }
        }

}

}else{
if(pattern<223){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return c[4]; }
        } else {
                { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }
        }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return c[4]; }
        } else {
                { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        }

}else{

        { if(pixel==2) return (c[4]*3+c[6]*1)/4; }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return c[4]; }
        } else {
                { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }
        }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return c[4]; }
        } else {
                { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return c[4]; }
        } else {
                { if(pixel==1) return (c[4]*14+c[1]*1+c[5]*1)/16; }
        }

}

}

}

}

}

}else{
if(pattern<240){
if(pattern<232){
if(pattern<228){
if(pattern<226){
if(pattern<225){

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[5]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[5]*1)/4; }

}

}else{
if(pattern<227){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[5]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[5]*1)/4; }

}

}

}else{
if(pattern<230){
if(pattern<229){

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[5]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[5]*1)/4; }

}

}else{
if(pattern<231){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[5]*1)/4; }

}else{

        { if(pixel==0) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[5]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[5]*1)/4; }

}

}

}

}else{
if(pattern<236){
if(pattern<234){
if(pattern<233){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return c[4]; }
                { if(pixel==3) return (c[4]*3+c[5]*1)/4; }
        } else {
                { if(pixel==2) return (c[3]*3+c[7]*3+c[4]*2)/8; }
                { if(pixel==3) return (c[4]*5+c[7]*2+c[5]*1)/8; }
        }

}else{

        { if(pixel==0) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[5]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return c[4]; }
        } else {
                { if(pixel==2) return (c[4]*14+c[3]*1+c[7]*1)/16; }
        }

}

}else{
if(pattern<235){

        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[5]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return c[4]; }
        } else {
                { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        } else {
                { if(pixel==0) return (c[4]*6+c[1]*1+c[3]*1)/8; }
        }

}else{

        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[5]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return c[4]; }
        } else {
                { if(pixel==2) return (c[4]*14+c[3]*1+c[7]*1)/16; }
        }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return c[4]; }
        } else {
                { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        }

}

}

}else{
if(pattern<238){
if(pattern<237){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return c[4]; }
                { if(pixel==3) return (c[4]*3+c[5]*1)/4; }
        } else {
                { if(pixel==2) return (c[3]*3+c[7]*3+c[4]*2)/8; }
                { if(pixel==3) return (c[4]*5+c[7]*2+c[5]*1)/8; }
        }

}else{

        { if(pixel==0) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[5]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return c[4]; }
        } else {
                { if(pixel==2) return (c[4]*14+c[3]*1+c[7]*1)/16; }
        }

}

}else{
if(pattern<239){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[5]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return c[4]; }
                { if(pixel==3) return (c[4]*3+c[5]*1)/4; }
        } else {
                { if(pixel==2) return (c[3]*3+c[7]*3+c[4]*2)/8; }
                { if(pixel==3) return (c[4]*5+c[7]*2+c[5]*1)/8; }
        }

}else{

        { if(pixel==1) return (c[4]*3+c[5]*1)/4; }
        { if(pixel==3) return (c[4]*3+c[5]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return c[4]; }
        } else {
                { if(pixel==2) return (c[4]*14+c[3]*1+c[7]*1)/16; }
        }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return c[4]; }
        } else {
                { if(pixel==0) return (c[4]*14+c[1]*1+c[3]*1)/16; }
        }

}

}

}

}

}else{
if(pattern<248){
if(pattern<244){
if(pattern<242){
if(pattern<241){

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        if (diff2(c[5],c[7])) {
                { if(pixel==2) return (c[4]*3+c[3]*1)/4; }
                { if(pixel==3) return c[4]; }
        } else {
                { if(pixel==2) return (c[4]*5+c[7]*2+c[3]*1)/8; }
                { if(pixel==3) return (c[5]*3+c[7]*3+c[4]*2)/8; }
        }

}else{

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        if (diff2(c[5],c[7])) {
                { if(pixel==2) return (c[4]*3+c[3]*1)/4; }
                { if(pixel==3) return c[4]; }
        } else {
                { if(pixel==2) return (c[4]*5+c[7]*2+c[3]*1)/8; }
                { if(pixel==3) return (c[5]*3+c[7]*3+c[4]*2)/8; }
        }

}

}else{
if(pattern<243){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[3]*1)/4; }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return c[4]; }
        } else {
                { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }
        }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        } else {
                { if(pixel==1) return (c[4]*6+c[1]*1+c[5]*1)/8; }
        }

}else{

        { if(pixel==0) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        if (diff2(c[5],c[7])) {
                { if(pixel==2) return (c[4]*3+c[3]*1)/4; }
                { if(pixel==3) return c[4]; }
        } else {
                { if(pixel==2) return (c[4]*5+c[7]*2+c[3]*1)/8; }
                { if(pixel==3) return (c[5]*3+c[7]*3+c[4]*2)/8; }
        }

}

}

}else{
if(pattern<246){
if(pattern<245){

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[3]*1)/4; }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return c[4]; }
        } else {
                { if(pixel==3) return (c[4]*14+c[5]*1+c[7]*1)/16; }
        }

}else{

        { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[3]*1)/4; }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return c[4]; }
        } else {
                { if(pixel==3) return (c[4]*14+c[5]*1+c[7]*1)/16; }
        }

}

}else{
if(pattern<247){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[3]*1)/4; }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return c[4]; }
        } else {
                { if(pixel==3) return (c[4]*14+c[5]*1+c[7]*1)/16; }
        }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return c[4]; }
        } else {
                { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        }

}else{

        { if(pixel==0) return (c[4]*3+c[3]*1)/4; }
        { if(pixel==2) return (c[4]*3+c[3]*1)/4; }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return c[4]; }
        } else {
                { if(pixel==3) return (c[4]*14+c[5]*1+c[7]*1)/16; }
        }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return c[4]; }
        } else {
                { if(pixel==1) return (c[4]*14+c[1]*1+c[5]*1)/16; }
        }

}

}

}

}else{
if(pattern<252){
if(pattern<250){
if(pattern<249){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return c[4]; }
        } else {
                { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return c[4]; }
        } else {
                { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }
        }

}else{

        { if(pixel==0) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return c[4]; }
        } else {
                { if(pixel==2) return (c[4]*14+c[3]*1+c[7]*1)/16; }
        }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return c[4]; }
        } else {
                { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }
        }

}

}else{
if(pattern<251){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return c[4]; }
        } else {
                { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return c[4]; }
        } else {
                { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }
        }

}else{

        { if(pixel==1) return (c[4]*3+c[2]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return c[4]; }
        } else {
                { if(pixel==2) return (c[4]*14+c[3]*1+c[7]*1)/16; }
        }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return c[4]; }
        } else {
                { if(pixel==3) return (c[4]*2+c[5]*1+c[7]*1)/4; }
        }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return c[4]; }
        } else {
                { if(pixel==0) return (c[4]*2+c[1]*1+c[3]*1)/4; }
        }

}

}

}else{
if(pattern<254){
if(pattern<253){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[1]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return c[4]; }
        } else {
                { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return c[4]; }
        } else {
                { if(pixel==3) return (c[4]*14+c[5]*1+c[7]*1)/16; }
        }

}else{

        { if(pixel==0) return (c[4]*3+c[1]*1)/4; }
        { if(pixel==1) return (c[4]*3+c[1]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return c[4]; }
        } else {
                { if(pixel==2) return (c[4]*14+c[3]*1+c[7]*1)/16; }
        }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return c[4]; }
        } else {
                { if(pixel==3) return (c[4]*14+c[5]*1+c[7]*1)/16; }
        }

}

}else{
if(pattern<255){

        { if(pixel==0) return (c[4]*3+c[0]*1)/4; }
        if (diff2(c[7],c[3])) {
                { if(pixel==2) return c[4]; }
        } else {
                { if(pixel==2) return (c[4]*2+c[3]*1+c[7]*1)/4; }
        }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return c[4]; }
        } else {
                { if(pixel==3) return (c[4]*14+c[5]*1+c[7]*1)/16; }
        }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return c[4]; }
        } else {
                { if(pixel==1) return (c[4]*2+c[1]*1+c[5]*1)/4; }
        }

}else{

        if (diff2(c[7],c[3])) {
                { if(pixel==2) return c[4]; }
        } else {
                { if(pixel==2) return (c[4]*14+c[3]*1+c[7]*1)/16; }
        }
        if (diff2(c[5],c[7])) {
                { if(pixel==3) return c[4]; }
        } else {
                { if(pixel==3) return (c[4]*14+c[5]*1+c[7]*1)/16; }
        }
        if (diff2(c[3],c[1])) {
                { if(pixel==0) return c[4]; }
        } else {
                { if(pixel==0) return (c[4]*14+c[1]*1+c[3]*1)/16; }
        }
        if (diff2(c[1],c[5])) {
                { if(pixel==1) return c[4]; }
        } else {
                { if(pixel==1) return (c[4]*14+c[1]*1+c[5]*1)/16; }
        }

}

}

}

}

}

}

}

}
return c[4];
}
void main(){
 ivec3 c[10]; int yy[10]; for(int j=0;j<10;j++){c[j]=ivec3(0);yy[j]=0;}
 vec2 uv=(floor(texCoord*texSize)+0.5)/texSize;
 c[0]=ivec3(floor(texture2D(tex,uv+vec2(-1.0,-1.0)/texSize).rgb*255.0+0.5)); yy[0]=yuv(c[0]);
 c[1]=ivec3(floor(texture2D(tex,uv+vec2(0.0,-1.0)/texSize).rgb*255.0+0.5)); yy[1]=yuv(c[1]);
 c[2]=ivec3(floor(texture2D(tex,uv+vec2(1.0,-1.0)/texSize).rgb*255.0+0.5)); yy[2]=yuv(c[2]);
 c[3]=ivec3(floor(texture2D(tex,uv+vec2(-1.0,0.0)/texSize).rgb*255.0+0.5)); yy[3]=yuv(c[3]);
 c[4]=ivec3(floor(texture2D(tex,uv+vec2(0.0,0.0)/texSize).rgb*255.0+0.5)); yy[4]=yuv(c[4]);
 c[5]=ivec3(floor(texture2D(tex,uv+vec2(1.0,0.0)/texSize).rgb*255.0+0.5)); yy[5]=yuv(c[5]);
 c[6]=ivec3(floor(texture2D(tex,uv+vec2(-1.0,1.0)/texSize).rgb*255.0+0.5)); yy[6]=yuv(c[6]);
 c[7]=ivec3(floor(texture2D(tex,uv+vec2(0.0,1.0)/texSize).rgb*255.0+0.5)); yy[7]=yuv(c[7]);
 c[8]=ivec3(floor(texture2D(tex,uv+vec2(1.0,1.0)/texSize).rgb*255.0+0.5)); yy[8]=yuv(c[8]);
int pattern=0;
if(diff2(c[4],c[0])) pattern|=1;
if(diff2(c[4],c[1])) pattern|=2;
if(diff2(c[4],c[2])) pattern|=4;
if(diff2(c[4],c[3])) pattern|=8;
if(diff2(c[4],c[5])) pattern|=16;
if(diff2(c[4],c[6])) pattern|=32;
if(diff2(c[4],c[7])) pattern|=64;
if(diff2(c[4],c[8])) pattern|=128;
ivec2 sub=ivec2(floor(fract(texCoord*texSize)*2.0)); int pixel=sub.y*2+sub.x; ivec3 result=c[4];
gl_FragColor=vec4(vec3(calculate(c,yy,pixel,pattern))/255.0,1.0);
}
