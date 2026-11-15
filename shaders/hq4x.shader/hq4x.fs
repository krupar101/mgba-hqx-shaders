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
#define PIXEL00_0 { if(pixel==0) return c[5]; }
#define PIXEL00_11 { if(pixel==0) return (c[5]*3+c[4]*1)/4; }
#define PIXEL00_12 { if(pixel==0) return (c[5]*3+c[2]*1)/4; }
#define PIXEL00_20 { if(pixel==0) return (c[5]*2+c[2]*1+c[4]*1)/4; }
#define PIXEL00_50 { if(pixel==0) return (c[2]*1+c[4]*1)/2; }
#define PIXEL00_80 { if(pixel==0) return (c[5]*5+c[1]*3)/8; }
#define PIXEL00_81 { if(pixel==0) return (c[5]*5+c[4]*3)/8; }
#define PIXEL00_82 { if(pixel==0) return (c[5]*5+c[2]*3)/8; }
#define PIXEL01_0 { if(pixel==1) return c[5]; }
#define PIXEL01_10 { if(pixel==1) return (c[5]*3+c[1]*1)/4; }
#define PIXEL01_12 { if(pixel==1) return (c[5]*3+c[2]*1)/4; }
#define PIXEL01_14 { if(pixel==1) return (c[2]*3+c[5]*1)/4; }
#define PIXEL01_21 { if(pixel==1) return (c[2]*2+c[5]*1+c[4]*1)/4; }
#define PIXEL01_31 { if(pixel==1) return (c[5]*7+c[4]*1)/8; }
#define PIXEL01_50 { if(pixel==1) return (c[2]*1+c[5]*1)/2; }
#define PIXEL01_60 { if(pixel==1) return (c[5]*5+c[2]*2+c[4]*1)/8; }
#define PIXEL01_61 { if(pixel==1) return (c[5]*5+c[2]*2+c[1]*1)/8; }
#define PIXEL01_82 { if(pixel==1) return (c[5]*5+c[2]*3)/8; }
#define PIXEL01_83 { if(pixel==1) return (c[2]*5+c[4]*3)/8; }
#define PIXEL02_0 { if(pixel==2) return c[5]; }
#define PIXEL02_10 { if(pixel==2) return (c[5]*3+c[3]*1)/4; }
#define PIXEL02_11 { if(pixel==2) return (c[5]*3+c[2]*1)/4; }
#define PIXEL02_13 { if(pixel==2) return (c[2]*3+c[5]*1)/4; }
#define PIXEL02_21 { if(pixel==2) return (c[2]*2+c[5]*1+c[6]*1)/4; }
#define PIXEL02_32 { if(pixel==2) return (c[5]*7+c[6]*1)/8; }
#define PIXEL02_50 { if(pixel==2) return (c[2]*1+c[5]*1)/2; }
#define PIXEL02_60 { if(pixel==2) return (c[5]*5+c[2]*2+c[6]*1)/8; }
#define PIXEL02_61 { if(pixel==2) return (c[5]*5+c[2]*2+c[3]*1)/8; }
#define PIXEL02_81 { if(pixel==2) return (c[5]*5+c[2]*3)/8; }
#define PIXEL02_83 { if(pixel==2) return (c[2]*5+c[6]*3)/8; }
#define PIXEL03_0 { if(pixel==3) return c[5]; }
#define PIXEL03_11 { if(pixel==3) return (c[5]*3+c[2]*1)/4; }
#define PIXEL03_12 { if(pixel==3) return (c[5]*3+c[6]*1)/4; }
#define PIXEL03_20 { if(pixel==3) return (c[5]*2+c[2]*1+c[6]*1)/4; }
#define PIXEL03_50 { if(pixel==3) return (c[2]*1+c[6]*1)/2; }
#define PIXEL03_80 { if(pixel==3) return (c[5]*5+c[3]*3)/8; }
#define PIXEL03_81 { if(pixel==3) return (c[5]*5+c[2]*3)/8; }
#define PIXEL03_82 { if(pixel==3) return (c[5]*5+c[6]*3)/8; }
#define PIXEL10_0 { if(pixel==4) return c[5]; }
#define PIXEL10_10 { if(pixel==4) return (c[5]*3+c[1]*1)/4; }
#define PIXEL10_11 { if(pixel==4) return (c[5]*3+c[4]*1)/4; }
#define PIXEL10_13 { if(pixel==4) return (c[4]*3+c[5]*1)/4; }
#define PIXEL10_21 { if(pixel==4) return (c[4]*2+c[5]*1+c[2]*1)/4; }
#define PIXEL10_32 { if(pixel==4) return (c[5]*7+c[2]*1)/8; }
#define PIXEL10_50 { if(pixel==4) return (c[4]*1+c[5]*1)/2; }
#define PIXEL10_60 { if(pixel==4) return (c[5]*5+c[4]*2+c[2]*1)/8; }
#define PIXEL10_61 { if(pixel==4) return (c[5]*5+c[4]*2+c[1]*1)/8; }
#define PIXEL10_81 { if(pixel==4) return (c[5]*5+c[4]*3)/8; }
#define PIXEL10_83 { if(pixel==4) return (c[4]*5+c[2]*3)/8; }
#define PIXEL11_0 { if(pixel==5) return c[5]; }
#define PIXEL11_30 { if(pixel==5) return (c[5]*7+c[1]*1)/8; }
#define PIXEL11_31 { if(pixel==5) return (c[5]*7+c[4]*1)/8; }
#define PIXEL11_32 { if(pixel==5) return (c[5]*7+c[2]*1)/8; }
#define PIXEL11_70 { if(pixel==5) return (c[5]*6+c[4]*1+c[2]*1)/8; }
#define PIXEL12_0 { if(pixel==6) return c[5]; }
#define PIXEL12_30 { if(pixel==6) return (c[5]*7+c[3]*1)/8; }
#define PIXEL12_31 { if(pixel==6) return (c[5]*7+c[2]*1)/8; }
#define PIXEL12_32 { if(pixel==6) return (c[5]*7+c[6]*1)/8; }
#define PIXEL12_70 { if(pixel==6) return (c[5]*6+c[6]*1+c[2]*1)/8; }
#define PIXEL13_0 { if(pixel==7) return c[5]; }
#define PIXEL13_10 { if(pixel==7) return (c[5]*3+c[3]*1)/4; }
#define PIXEL13_12 { if(pixel==7) return (c[5]*3+c[6]*1)/4; }
#define PIXEL13_14 { if(pixel==7) return (c[6]*3+c[5]*1)/4; }
#define PIXEL13_21 { if(pixel==7) return (c[6]*2+c[5]*1+c[2]*1)/4; }
#define PIXEL13_31 { if(pixel==7) return (c[5]*7+c[2]*1)/8; }
#define PIXEL13_50 { if(pixel==7) return (c[6]*1+c[5]*1)/2; }
#define PIXEL13_60 { if(pixel==7) return (c[5]*5+c[6]*2+c[2]*1)/8; }
#define PIXEL13_61 { if(pixel==7) return (c[5]*5+c[6]*2+c[3]*1)/8; }
#define PIXEL13_82 { if(pixel==7) return (c[5]*5+c[6]*3)/8; }
#define PIXEL13_83 { if(pixel==7) return (c[6]*5+c[2]*3)/8; }
#define PIXEL20_0 { if(pixel==8) return c[5]; }
#define PIXEL20_10 { if(pixel==8) return (c[5]*3+c[7]*1)/4; }
#define PIXEL20_12 { if(pixel==8) return (c[5]*3+c[4]*1)/4; }
#define PIXEL20_14 { if(pixel==8) return (c[4]*3+c[5]*1)/4; }
#define PIXEL20_21 { if(pixel==8) return (c[4]*2+c[5]*1+c[8]*1)/4; }
#define PIXEL20_31 { if(pixel==8) return (c[5]*7+c[8]*1)/8; }
#define PIXEL20_50 { if(pixel==8) return (c[4]*1+c[5]*1)/2; }
#define PIXEL20_60 { if(pixel==8) return (c[5]*5+c[4]*2+c[8]*1)/8; }
#define PIXEL20_61 { if(pixel==8) return (c[5]*5+c[4]*2+c[7]*1)/8; }
#define PIXEL20_82 { if(pixel==8) return (c[5]*5+c[4]*3)/8; }
#define PIXEL20_83 { if(pixel==8) return (c[4]*5+c[8]*3)/8; }
#define PIXEL21_0 { if(pixel==9) return c[5]; }
#define PIXEL21_30 { if(pixel==9) return (c[5]*7+c[7]*1)/8; }
#define PIXEL21_31 { if(pixel==9) return (c[5]*7+c[8]*1)/8; }
#define PIXEL21_32 { if(pixel==9) return (c[5]*7+c[4]*1)/8; }
#define PIXEL21_70 { if(pixel==9) return (c[5]*6+c[4]*1+c[8]*1)/8; }
#define PIXEL22_0 { if(pixel==10) return c[5]; }
#define PIXEL22_30 { if(pixel==10) return (c[5]*7+c[9]*1)/8; }
#define PIXEL22_31 { if(pixel==10) return (c[5]*7+c[6]*1)/8; }
#define PIXEL22_32 { if(pixel==10) return (c[5]*7+c[8]*1)/8; }
#define PIXEL22_70 { if(pixel==10) return (c[5]*6+c[6]*1+c[8]*1)/8; }
#define PIXEL23_0 { if(pixel==11) return c[5]; }
#define PIXEL23_10 { if(pixel==11) return (c[5]*3+c[9]*1)/4; }
#define PIXEL23_11 { if(pixel==11) return (c[5]*3+c[6]*1)/4; }
#define PIXEL23_13 { if(pixel==11) return (c[6]*3+c[5]*1)/4; }
#define PIXEL23_21 { if(pixel==11) return (c[6]*2+c[5]*1+c[8]*1)/4; }
#define PIXEL23_32 { if(pixel==11) return (c[5]*7+c[8]*1)/8; }
#define PIXEL23_50 { if(pixel==11) return (c[6]*1+c[5]*1)/2; }
#define PIXEL23_60 { if(pixel==11) return (c[5]*5+c[6]*2+c[8]*1)/8; }
#define PIXEL23_61 { if(pixel==11) return (c[5]*5+c[6]*2+c[9]*1)/8; }
#define PIXEL23_81 { if(pixel==11) return (c[5]*5+c[6]*3)/8; }
#define PIXEL23_83 { if(pixel==11) return (c[6]*5+c[8]*3)/8; }
#define PIXEL30_0 { if(pixel==12) return c[5]; }
#define PIXEL30_11 { if(pixel==12) return (c[5]*3+c[8]*1)/4; }
#define PIXEL30_12 { if(pixel==12) return (c[5]*3+c[4]*1)/4; }
#define PIXEL30_20 { if(pixel==12) return (c[5]*2+c[8]*1+c[4]*1)/4; }
#define PIXEL30_50 { if(pixel==12) return (c[8]*1+c[4]*1)/2; }
#define PIXEL30_80 { if(pixel==12) return (c[5]*5+c[7]*3)/8; }
#define PIXEL30_81 { if(pixel==12) return (c[5]*5+c[8]*3)/8; }
#define PIXEL30_82 { if(pixel==12) return (c[5]*5+c[4]*3)/8; }
#define PIXEL31_0 { if(pixel==13) return c[5]; }
#define PIXEL31_10 { if(pixel==13) return (c[5]*3+c[7]*1)/4; }
#define PIXEL31_11 { if(pixel==13) return (c[5]*3+c[8]*1)/4; }
#define PIXEL31_13 { if(pixel==13) return (c[8]*3+c[5]*1)/4; }
#define PIXEL31_21 { if(pixel==13) return (c[8]*2+c[5]*1+c[4]*1)/4; }
#define PIXEL31_32 { if(pixel==13) return (c[5]*7+c[4]*1)/8; }
#define PIXEL31_50 { if(pixel==13) return (c[8]*1+c[5]*1)/2; }
#define PIXEL31_60 { if(pixel==13) return (c[5]*5+c[8]*2+c[4]*1)/8; }
#define PIXEL31_61 { if(pixel==13) return (c[5]*5+c[8]*2+c[7]*1)/8; }
#define PIXEL31_81 { if(pixel==13) return (c[5]*5+c[8]*3)/8; }
#define PIXEL31_83 { if(pixel==13) return (c[8]*5+c[4]*3)/8; }
#define PIXEL32_0 { if(pixel==14) return c[5]; }
#define PIXEL32_10 { if(pixel==14) return (c[5]*3+c[9]*1)/4; }
#define PIXEL32_12 { if(pixel==14) return (c[5]*3+c[8]*1)/4; }
#define PIXEL32_14 { if(pixel==14) return (c[8]*3+c[5]*1)/4; }
#define PIXEL32_21 { if(pixel==14) return (c[8]*2+c[5]*1+c[6]*1)/4; }
#define PIXEL32_31 { if(pixel==14) return (c[5]*7+c[6]*1)/8; }
#define PIXEL32_50 { if(pixel==14) return (c[8]*1+c[5]*1)/2; }
#define PIXEL32_60 { if(pixel==14) return (c[5]*5+c[8]*2+c[6]*1)/8; }
#define PIXEL32_61 { if(pixel==14) return (c[5]*5+c[8]*2+c[9]*1)/8; }
#define PIXEL32_82 { if(pixel==14) return (c[5]*5+c[8]*3)/8; }
#define PIXEL32_83 { if(pixel==14) return (c[8]*5+c[6]*3)/8; }
#define PIXEL33_0 { if(pixel==15) return c[5]; }
#define PIXEL33_11 { if(pixel==15) return (c[5]*3+c[6]*1)/4; }
#define PIXEL33_12 { if(pixel==15) return (c[5]*3+c[8]*1)/4; }
#define PIXEL33_20 { if(pixel==15) return (c[5]*2+c[8]*1+c[6]*1)/4; }
#define PIXEL33_50 { if(pixel==15) return (c[8]*1+c[6]*1)/2; }
#define PIXEL33_80 { if(pixel==15) return (c[5]*5+c[9]*3)/8; }
#define PIXEL33_81 { if(pixel==15) return (c[5]*5+c[6]*3)/8; }
#define PIXEL33_82 { if(pixel==15) return (c[5]*5+c[8]*3)/8; }
/*
    VisualBoyAdvance - a Game Boy & Game Boy Advance emulator

        Copyright (C) 1999 - 2003 Forgotten
    Copyright (C) 2003 - 2004 Forgotten and the VBA development team
    Copyright (C) 2005 - 2006 VBA development team


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
*/


if(pattern<128){
if(pattern<64){
if(pattern<32){
if(pattern<16){
if(pattern<8){
if(pattern<4){
if(pattern<2){
if(pattern<1){

        PIXEL00_20
        PIXEL01_60
        PIXEL02_60
        PIXEL03_20
        PIXEL10_60
        PIXEL11_70
        PIXEL12_70
        PIXEL13_60
        PIXEL20_60
        PIXEL21_70
        PIXEL22_70
        PIXEL23_60
        PIXEL30_20
        PIXEL31_60
        PIXEL32_60
        PIXEL33_20
        

}else{

        PIXEL00_20
        PIXEL01_60
        PIXEL02_60
        PIXEL03_20
        PIXEL10_60
        PIXEL11_70
        PIXEL12_70
        PIXEL13_60
        PIXEL20_60
        PIXEL21_70
        PIXEL22_70
        PIXEL23_60
        PIXEL30_20
        PIXEL31_60
        PIXEL32_60
        PIXEL33_20
        

}

}else{
if(pattern<3){

        PIXEL00_80
        PIXEL01_10
        PIXEL02_10
        PIXEL03_80
        PIXEL10_61
        PIXEL11_30
        PIXEL12_30
        PIXEL13_61
        PIXEL20_60
        PIXEL21_70
        PIXEL22_70
        PIXEL23_60
        PIXEL30_20
        PIXEL31_60
        PIXEL32_60
        PIXEL33_20
        

}else{

        PIXEL00_81
        PIXEL01_31
        PIXEL02_10
        PIXEL03_80
        PIXEL10_81
        PIXEL11_31
        PIXEL12_30
        PIXEL13_61
        PIXEL20_60
        PIXEL21_70
        PIXEL22_70
        PIXEL23_60
        PIXEL30_20
        PIXEL31_60
        PIXEL32_60
        PIXEL33_20
        

}

}

}else{
if(pattern<6){
if(pattern<5){

        PIXEL00_20
        PIXEL01_60
        PIXEL02_60
        PIXEL03_20
        PIXEL10_60
        PIXEL11_70
        PIXEL12_70
        PIXEL13_60
        PIXEL20_60
        PIXEL21_70
        PIXEL22_70
        PIXEL23_60
        PIXEL30_20
        PIXEL31_60
        PIXEL32_60
        PIXEL33_20
        

}else{

        PIXEL00_20
        PIXEL01_60
        PIXEL02_60
        PIXEL03_20
        PIXEL10_60
        PIXEL11_70
        PIXEL12_70
        PIXEL13_60
        PIXEL20_60
        PIXEL21_70
        PIXEL22_70
        PIXEL23_60
        PIXEL30_20
        PIXEL31_60
        PIXEL32_60
        PIXEL33_20
        

}

}else{
if(pattern<7){

        PIXEL00_80
        PIXEL01_10
        PIXEL02_32
        PIXEL03_82
        PIXEL10_61
        PIXEL11_30
        PIXEL12_32
        PIXEL13_82
        PIXEL20_60
        PIXEL21_70
        PIXEL22_70
        PIXEL23_60
        PIXEL30_20
        PIXEL31_60
        PIXEL32_60
        PIXEL33_20
        

}else{

        PIXEL00_81
        PIXEL01_31
        PIXEL02_32
        PIXEL03_82
        PIXEL10_81
        PIXEL11_31
        PIXEL12_32
        PIXEL13_82
        PIXEL20_60
        PIXEL21_70
        PIXEL22_70
        PIXEL23_60
        PIXEL30_20
        PIXEL31_60
        PIXEL32_60
        PIXEL33_20
        

}

}

}

}else{
if(pattern<12){
if(pattern<10){
if(pattern<9){

        PIXEL00_80
        PIXEL01_61
        PIXEL02_60
        PIXEL03_20
        PIXEL10_10
        PIXEL11_30
        PIXEL12_70
        PIXEL13_60
        PIXEL20_10
        PIXEL21_30
        PIXEL22_70
        PIXEL23_60
        PIXEL30_80
        PIXEL31_61
        PIXEL32_60
        PIXEL33_20
        

}else{

        PIXEL00_82
        PIXEL01_82
        PIXEL02_60
        PIXEL03_20
        PIXEL10_32
        PIXEL11_32
        PIXEL12_70
        PIXEL13_60
        PIXEL20_10
        PIXEL21_30
        PIXEL22_70
        PIXEL23_60
        PIXEL30_80
        PIXEL31_61
        PIXEL32_60
        PIXEL33_20
        

}

}else{
if(pattern<11){

        if (Diff(yy[4], yy[2])) {
                PIXEL00_80
                PIXEL01_10
                PIXEL10_10
                PIXEL11_30
        } else {
                PIXEL00_50
                PIXEL01_50
                PIXEL10_50
                PIXEL11_0
        }
        PIXEL02_10
        PIXEL03_80
        PIXEL12_30
        PIXEL13_61
        PIXEL20_10
        PIXEL21_30
        PIXEL22_70
        PIXEL23_60
        PIXEL30_80
        PIXEL31_61
        PIXEL32_60
        PIXEL33_20
        

}else{

        if (Diff(yy[4], yy[2])) {
                PIXEL00_0
                PIXEL01_0
                PIXEL10_0
        } else {
                PIXEL00_50
                PIXEL01_50
                PIXEL10_50
        }
        PIXEL02_10
        PIXEL03_80
        PIXEL11_0
        PIXEL12_30
        PIXEL13_61
        PIXEL20_10
        PIXEL21_30
        PIXEL22_70
        PIXEL23_60
        PIXEL30_80
        PIXEL31_61
        PIXEL32_60
        PIXEL33_20
        

}

}

}else{
if(pattern<14){
if(pattern<13){

        PIXEL00_80
        PIXEL01_61
        PIXEL02_60
        PIXEL03_20
        PIXEL10_10
        PIXEL11_30
        PIXEL12_70
        PIXEL13_60
        PIXEL20_10
        PIXEL21_30
        PIXEL22_70
        PIXEL23_60
        PIXEL30_80
        PIXEL31_61
        PIXEL32_60
        PIXEL33_20
        

}else{

        PIXEL00_82
        PIXEL01_82
        PIXEL02_60
        PIXEL03_20
        PIXEL10_32
        PIXEL11_32
        PIXEL12_70
        PIXEL13_60
        PIXEL20_10
        PIXEL21_30
        PIXEL22_70
        PIXEL23_60
        PIXEL30_80
        PIXEL31_61
        PIXEL32_60
        PIXEL33_20
        

}

}else{
if(pattern<15){

        if (Diff(yy[4], yy[2])) {
                PIXEL00_80
                PIXEL01_10
                PIXEL02_32
                PIXEL03_82
                PIXEL10_10
                PIXEL11_30
        } else {
                PIXEL00_50
                PIXEL01_83
                PIXEL02_13
                PIXEL03_11
                PIXEL10_21
                PIXEL11_70
        }
        PIXEL12_32
        PIXEL13_82
        PIXEL20_10
        PIXEL21_30
        PIXEL22_70
        PIXEL23_60
        PIXEL30_80
        PIXEL31_61
        PIXEL32_60
        PIXEL33_20
        

}else{

        if (Diff(yy[4], yy[2])) {
                PIXEL00_0
                PIXEL01_0
                PIXEL02_32
                PIXEL03_82
                PIXEL10_0
                PIXEL11_0
        } else {
                PIXEL00_50
                PIXEL01_83
                PIXEL02_13
                PIXEL03_11
                PIXEL10_21
                PIXEL11_70
        }
        PIXEL12_32
        PIXEL13_82
        PIXEL20_10
        PIXEL21_30
        PIXEL22_70
        PIXEL23_60
        PIXEL30_80
        PIXEL31_61
        PIXEL32_60
        PIXEL33_20
        

}

}

}

}

}else{
if(pattern<24){
if(pattern<20){
if(pattern<18){
if(pattern<17){

        PIXEL00_20
        PIXEL01_60
        PIXEL02_61
        PIXEL03_80
        PIXEL10_60
        PIXEL11_70
        PIXEL12_30
        PIXEL13_10
        PIXEL20_60
        PIXEL21_70
        PIXEL22_30
        PIXEL23_10
        PIXEL30_20
        PIXEL31_60
        PIXEL32_61
        PIXEL33_80
        

}else{

        PIXEL00_20
        PIXEL01_60
        PIXEL02_61
        PIXEL03_80
        PIXEL10_60
        PIXEL11_70
        PIXEL12_30
        PIXEL13_10
        PIXEL20_60
        PIXEL21_70
        PIXEL22_30
        PIXEL23_10
        PIXEL30_20
        PIXEL31_60
        PIXEL32_61
        PIXEL33_80
        

}

}else{
if(pattern<19){

        PIXEL00_80
        PIXEL01_10
        if (Diff(yy[2], yy[6])) {
                PIXEL02_10
                PIXEL03_80
                PIXEL12_30
                PIXEL13_10
        } else {
                PIXEL02_50
                PIXEL03_50
                PIXEL12_0
                PIXEL13_50
        }
        PIXEL10_61
        PIXEL11_30
        PIXEL20_60
        PIXEL21_70
        PIXEL22_30
        PIXEL23_10
        PIXEL30_20
        PIXEL31_60
        PIXEL32_61
        PIXEL33_80
        

}else{

        if (Diff(yy[2], yy[6])) {
                PIXEL00_81
                PIXEL01_31
                PIXEL02_10
                PIXEL03_80
                PIXEL12_30
                PIXEL13_10
        } else {
                PIXEL00_12
                PIXEL01_14
                PIXEL02_83
                PIXEL03_50
                PIXEL12_70
                PIXEL13_21
        }
        PIXEL10_81
        PIXEL11_31
        PIXEL20_60
        PIXEL21_70
        PIXEL22_30
        PIXEL23_10
        PIXEL30_20
        PIXEL31_60
        PIXEL32_61
        PIXEL33_80
        

}

}

}else{
if(pattern<22){
if(pattern<21){

        PIXEL00_20
        PIXEL01_60
        PIXEL02_81
        PIXEL03_81
        PIXEL10_60
        PIXEL11_70
        PIXEL12_31
        PIXEL13_31
        PIXEL20_60
        PIXEL21_70
        PIXEL22_30
        PIXEL23_10
        PIXEL30_20
        PIXEL31_60
        PIXEL32_61
        PIXEL33_80
        

}else{

        PIXEL00_20
        PIXEL01_60
        PIXEL02_81
        PIXEL03_81
        PIXEL10_60
        PIXEL11_70
        PIXEL12_31
        PIXEL13_31
        PIXEL20_60
        PIXEL21_70
        PIXEL22_30
        PIXEL23_10
        PIXEL30_20
        PIXEL31_60
        PIXEL32_61
        PIXEL33_80
        

}

}else{
if(pattern<23){

        PIXEL00_80
        PIXEL01_10
        if (Diff(yy[2], yy[6])) {
                PIXEL02_0
                PIXEL03_0
                PIXEL13_0
        } else {
                PIXEL02_50
                PIXEL03_50
                PIXEL13_50
        }
        PIXEL10_61
        PIXEL11_30
        PIXEL12_0
        PIXEL20_60
        PIXEL21_70
        PIXEL22_30
        PIXEL23_10
        PIXEL30_20
        PIXEL31_60
        PIXEL32_61
        PIXEL33_80
        

}else{

        if (Diff(yy[2], yy[6])) {
                PIXEL00_81
                PIXEL01_31
                PIXEL02_0
                PIXEL03_0
                PIXEL12_0
                PIXEL13_0
        } else {
                PIXEL00_12
                PIXEL01_14
                PIXEL02_83
                PIXEL03_50
                PIXEL12_70
                PIXEL13_21
        }
        PIXEL10_81
        PIXEL11_31
        PIXEL20_60
        PIXEL21_70
        PIXEL22_30
        PIXEL23_10
        PIXEL30_20
        PIXEL31_60
        PIXEL32_61
        PIXEL33_80
        

}

}

}

}else{
if(pattern<28){
if(pattern<26){
if(pattern<25){

        PIXEL00_80
        PIXEL01_61
        PIXEL02_61
        PIXEL03_80
        PIXEL10_10
        PIXEL11_30
        PIXEL12_30
        PIXEL13_10
        PIXEL20_10
        PIXEL21_30
        PIXEL22_30
        PIXEL23_10
        PIXEL30_80
        PIXEL31_61
        PIXEL32_61
        PIXEL33_80
        

}else{

        PIXEL00_82
        PIXEL01_82
        PIXEL02_61
        PIXEL03_80
        PIXEL10_32
        PIXEL11_32
        PIXEL12_30
        PIXEL13_10
        PIXEL20_10
        PIXEL21_30
        PIXEL22_30
        PIXEL23_10
        PIXEL30_80
        PIXEL31_61
        PIXEL32_61
        PIXEL33_80
        

}

}else{
if(pattern<27){

        if (Diff(yy[4], yy[2])) {
                PIXEL00_0
                PIXEL01_0
                PIXEL10_0
        } else {
                PIXEL00_50
                PIXEL01_50
                PIXEL10_50
        }
        if (Diff(yy[2], yy[6])) {
                PIXEL02_0
                PIXEL03_0
                PIXEL13_0
        } else {
                PIXEL02_50
                PIXEL03_50
                PIXEL13_50
        }
        PIXEL11_0
        PIXEL12_0
        PIXEL20_10
        PIXEL21_30
        PIXEL22_30
        PIXEL23_10
        PIXEL30_80
        PIXEL31_61
        PIXEL32_61
        PIXEL33_80
        

}else{

        if (Diff(yy[4], yy[2])) {
                PIXEL00_0
                PIXEL01_0
                PIXEL10_0
        } else {
                PIXEL00_50
                PIXEL01_50
                PIXEL10_50
        }
        PIXEL02_10
        PIXEL03_80
        PIXEL11_0
        PIXEL12_30
        PIXEL13_10
        PIXEL20_10
        PIXEL21_30
        PIXEL22_30
        PIXEL23_10
        PIXEL30_80
        PIXEL31_61
        PIXEL32_61
        PIXEL33_80
        

}

}

}else{
if(pattern<30){
if(pattern<29){

        PIXEL00_80
        PIXEL01_61
        PIXEL02_81
        PIXEL03_81
        PIXEL10_10
        PIXEL11_30
        PIXEL12_31
        PIXEL13_31
        PIXEL20_10
        PIXEL21_30
        PIXEL22_30
        PIXEL23_10
        PIXEL30_80
        PIXEL31_61
        PIXEL32_61
        PIXEL33_80
        

}else{

        PIXEL00_82
        PIXEL01_82
        PIXEL02_81
        PIXEL03_81
        PIXEL10_32
        PIXEL11_32
        PIXEL12_31
        PIXEL13_31
        PIXEL20_10
        PIXEL21_30
        PIXEL22_30
        PIXEL23_10
        PIXEL30_80
        PIXEL31_61
        PIXEL32_61
        PIXEL33_80
        

}

}else{
if(pattern<31){

        PIXEL00_80
        PIXEL01_10
        if (Diff(yy[2], yy[6])) {
                PIXEL02_0
                PIXEL03_0
                PIXEL13_0
        } else {
                PIXEL02_50
                PIXEL03_50
                PIXEL13_50
        }
        PIXEL10_10
        PIXEL11_30
        PIXEL12_0
        PIXEL20_10
        PIXEL21_30
        PIXEL22_30
        PIXEL23_10
        PIXEL30_80
        PIXEL31_61
        PIXEL32_61
        PIXEL33_80
        

}else{

        if (Diff(yy[4], yy[2])) {
                PIXEL00_0
                PIXEL01_0
                PIXEL10_0
        } else {
                PIXEL00_50
                PIXEL01_50
                PIXEL10_50
        }
        if (Diff(yy[2], yy[6])) {
                PIXEL02_0
                PIXEL03_0
                PIXEL13_0
        } else {
                PIXEL02_50
                PIXEL03_50
                PIXEL13_50
        }
        PIXEL11_0
        PIXEL12_0
        PIXEL20_10
        PIXEL21_30
        PIXEL22_30
        PIXEL23_10
        PIXEL30_80
        PIXEL31_61
        PIXEL32_61
        PIXEL33_80
        

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

        PIXEL00_20
        PIXEL01_60
        PIXEL02_60
        PIXEL03_20
        PIXEL10_60
        PIXEL11_70
        PIXEL12_70
        PIXEL13_60
        PIXEL20_60
        PIXEL21_70
        PIXEL22_70
        PIXEL23_60
        PIXEL30_20
        PIXEL31_60
        PIXEL32_60
        PIXEL33_20
        

}else{

        PIXEL00_20
        PIXEL01_60
        PIXEL02_60
        PIXEL03_20
        PIXEL10_60
        PIXEL11_70
        PIXEL12_70
        PIXEL13_60
        PIXEL20_60
        PIXEL21_70
        PIXEL22_70
        PIXEL23_60
        PIXEL30_20
        PIXEL31_60
        PIXEL32_60
        PIXEL33_20
        

}

}else{
if(pattern<35){

        PIXEL00_80
        PIXEL01_10
        PIXEL02_10
        PIXEL03_80
        PIXEL10_61
        PIXEL11_30
        PIXEL12_30
        PIXEL13_61
        PIXEL20_60
        PIXEL21_70
        PIXEL22_70
        PIXEL23_60
        PIXEL30_20
        PIXEL31_60
        PIXEL32_60
        PIXEL33_20
        

}else{

        PIXEL00_81
        PIXEL01_31
        PIXEL02_10
        PIXEL03_80
        PIXEL10_81
        PIXEL11_31
        PIXEL12_30
        PIXEL13_61
        PIXEL20_60
        PIXEL21_70
        PIXEL22_70
        PIXEL23_60
        PIXEL30_20
        PIXEL31_60
        PIXEL32_60
        PIXEL33_20
        

}

}

}else{
if(pattern<38){
if(pattern<37){

        PIXEL00_20
        PIXEL01_60
        PIXEL02_60
        PIXEL03_20
        PIXEL10_60
        PIXEL11_70
        PIXEL12_70
        PIXEL13_60
        PIXEL20_60
        PIXEL21_70
        PIXEL22_70
        PIXEL23_60
        PIXEL30_20
        PIXEL31_60
        PIXEL32_60
        PIXEL33_20
        

}else{

        PIXEL00_20
        PIXEL01_60
        PIXEL02_60
        PIXEL03_20
        PIXEL10_60
        PIXEL11_70
        PIXEL12_70
        PIXEL13_60
        PIXEL20_60
        PIXEL21_70
        PIXEL22_70
        PIXEL23_60
        PIXEL30_20
        PIXEL31_60
        PIXEL32_60
        PIXEL33_20
        

}

}else{
if(pattern<39){

        PIXEL00_80
        PIXEL01_10
        PIXEL02_32
        PIXEL03_82
        PIXEL10_61
        PIXEL11_30
        PIXEL12_32
        PIXEL13_82
        PIXEL20_60
        PIXEL21_70
        PIXEL22_70
        PIXEL23_60
        PIXEL30_20
        PIXEL31_60
        PIXEL32_60
        PIXEL33_20
        

}else{

        PIXEL00_81
        PIXEL01_31
        PIXEL02_32
        PIXEL03_82
        PIXEL10_81
        PIXEL11_31
        PIXEL12_32
        PIXEL13_82
        PIXEL20_60
        PIXEL21_70
        PIXEL22_70
        PIXEL23_60
        PIXEL30_20
        PIXEL31_60
        PIXEL32_60
        PIXEL33_20
        

}

}

}

}else{
if(pattern<44){
if(pattern<42){
if(pattern<41){

        PIXEL00_80
        PIXEL01_61
        PIXEL02_60
        PIXEL03_20
        PIXEL10_10
        PIXEL11_30
        PIXEL12_70
        PIXEL13_60
        PIXEL20_31
        PIXEL21_31
        PIXEL22_70
        PIXEL23_60
        PIXEL30_81
        PIXEL31_81
        PIXEL32_60
        PIXEL33_20
        

}else{

        PIXEL00_82
        PIXEL01_82
        PIXEL02_60
        PIXEL03_20
        PIXEL10_32
        PIXEL11_32
        PIXEL12_70
        PIXEL13_60
        PIXEL20_31
        PIXEL21_31
        PIXEL22_70
        PIXEL23_60
        PIXEL30_81
        PIXEL31_81
        PIXEL32_60
        PIXEL33_20
        

}

}else{
if(pattern<43){

        if (Diff(yy[4], yy[2])) {
                PIXEL00_80
                PIXEL01_10
                PIXEL10_10
                PIXEL11_30
                PIXEL20_31
                PIXEL30_81
        } else {
                PIXEL00_50
                PIXEL01_21
                PIXEL10_83
                PIXEL11_70
                PIXEL20_14
                PIXEL30_12
        }
        PIXEL02_10
        PIXEL03_80
        PIXEL12_30
        PIXEL13_61
        PIXEL21_31
        PIXEL22_70
        PIXEL23_60
        PIXEL31_81
        PIXEL32_60
        PIXEL33_20
        

}else{

        if (Diff(yy[4], yy[2])) {
                PIXEL00_0
                PIXEL01_0
                PIXEL10_0
                PIXEL11_0
                PIXEL20_31
                PIXEL30_81
        } else {
                PIXEL00_50
                PIXEL01_21
                PIXEL10_83
                PIXEL11_70
                PIXEL20_14
                PIXEL30_12
        }
        PIXEL02_10
        PIXEL03_80
        PIXEL12_30
        PIXEL13_61
        PIXEL21_31
        PIXEL22_70
        PIXEL23_60
        PIXEL31_81
        PIXEL32_60
        PIXEL33_20
        

}

}

}else{
if(pattern<46){
if(pattern<45){

        PIXEL00_80
        PIXEL01_61
        PIXEL02_60
        PIXEL03_20
        PIXEL10_10
        PIXEL11_30
        PIXEL12_70
        PIXEL13_60
        PIXEL20_31
        PIXEL21_31
        PIXEL22_70
        PIXEL23_60
        PIXEL30_81
        PIXEL31_81
        PIXEL32_60
        PIXEL33_20
        

}else{

        PIXEL00_82
        PIXEL01_82
        PIXEL02_60
        PIXEL03_20
        PIXEL10_32
        PIXEL11_32
        PIXEL12_70
        PIXEL13_60
        PIXEL20_31
        PIXEL21_31
        PIXEL22_70
        PIXEL23_60
        PIXEL30_81
        PIXEL31_81
        PIXEL32_60
        PIXEL33_20
        

}

}else{
if(pattern<47){

        if (Diff(yy[4], yy[2])) {
                PIXEL00_80
                PIXEL01_10
                PIXEL10_10
                PIXEL11_30
        } else {
                PIXEL00_20
                PIXEL01_12
                PIXEL10_11
                PIXEL11_0
        }
        PIXEL02_32
        PIXEL03_82
        PIXEL12_32
        PIXEL13_82
        PIXEL20_31
        PIXEL21_31
        PIXEL22_70
        PIXEL23_60
        PIXEL30_81
        PIXEL31_81
        PIXEL32_60
        PIXEL33_20
        

}else{

        if (Diff(yy[4], yy[2])) {
                PIXEL00_0
        } else {
                PIXEL00_20
        }
        PIXEL01_0
        PIXEL02_32
        PIXEL03_82
        PIXEL10_0
        PIXEL11_0
        PIXEL12_32
        PIXEL13_82
        PIXEL20_31
        PIXEL21_31
        PIXEL22_70
        PIXEL23_60
        PIXEL30_81
        PIXEL31_81
        PIXEL32_60
        PIXEL33_20
        

}

}

}

}

}else{
if(pattern<56){
if(pattern<52){
if(pattern<50){
if(pattern<49){

        PIXEL00_20
        PIXEL01_60
        PIXEL02_61
        PIXEL03_80
        PIXEL10_60
        PIXEL11_70
        PIXEL12_30
        PIXEL13_10
        PIXEL20_60
        PIXEL21_70
        PIXEL22_30
        PIXEL23_10
        PIXEL30_20
        PIXEL31_60
        PIXEL32_61
        PIXEL33_80
        

}else{

        PIXEL00_20
        PIXEL01_60
        PIXEL02_61
        PIXEL03_80
        PIXEL10_60
        PIXEL11_70
        PIXEL12_30
        PIXEL13_10
        PIXEL20_60
        PIXEL21_70
        PIXEL22_30
        PIXEL23_10
        PIXEL30_20
        PIXEL31_60
        PIXEL32_61
        PIXEL33_80
        

}

}else{
if(pattern<51){

        PIXEL00_80
        PIXEL01_10
        if (Diff(yy[2], yy[6])) {
                PIXEL02_10
                PIXEL03_80
                PIXEL12_30
                PIXEL13_10
        } else {
                PIXEL02_50
                PIXEL03_50
                PIXEL12_0
                PIXEL13_50
        }
        PIXEL10_61
        PIXEL11_30
        PIXEL20_60
        PIXEL21_70
        PIXEL22_30
        PIXEL23_10
        PIXEL30_20
        PIXEL31_60
        PIXEL32_61
        PIXEL33_80
        

}else{

        if (Diff(yy[2], yy[6])) {
                PIXEL00_81
                PIXEL01_31
                PIXEL02_10
                PIXEL03_80
                PIXEL12_30
                PIXEL13_10
        } else {
                PIXEL00_12
                PIXEL01_14
                PIXEL02_83
                PIXEL03_50
                PIXEL12_70
                PIXEL13_21
        }
        PIXEL10_81
        PIXEL11_31
        PIXEL20_60
        PIXEL21_70
        PIXEL22_30
        PIXEL23_10
        PIXEL30_20
        PIXEL31_60
        PIXEL32_61
        PIXEL33_80
        

}

}

}else{
if(pattern<54){
if(pattern<53){

        PIXEL00_20
        PIXEL01_60
        PIXEL02_81
        PIXEL03_81
        PIXEL10_60
        PIXEL11_70
        PIXEL12_31
        PIXEL13_31
        PIXEL20_60
        PIXEL21_70
        PIXEL22_30
        PIXEL23_10
        PIXEL30_20
        PIXEL31_60
        PIXEL32_61
        PIXEL33_80
        

}else{

        PIXEL00_20
        PIXEL01_60
        PIXEL02_81
        PIXEL03_81
        PIXEL10_60
        PIXEL11_70
        PIXEL12_31
        PIXEL13_31
        PIXEL20_60
        PIXEL21_70
        PIXEL22_30
        PIXEL23_10
        PIXEL30_20
        PIXEL31_60
        PIXEL32_61
        PIXEL33_80
        

}

}else{
if(pattern<55){

        PIXEL00_80
        PIXEL01_10
        if (Diff(yy[2], yy[6])) {
                PIXEL02_0
                PIXEL03_0
                PIXEL13_0
        } else {
                PIXEL02_50
                PIXEL03_50
                PIXEL13_50
        }
        PIXEL10_61
        PIXEL11_30
        PIXEL12_0
        PIXEL20_60
        PIXEL21_70
        PIXEL22_30
        PIXEL23_10
        PIXEL30_20
        PIXEL31_60
        PIXEL32_61
        PIXEL33_80
        

}else{

        if (Diff(yy[2], yy[6])) {
                PIXEL00_81
                PIXEL01_31
                PIXEL02_0
                PIXEL03_0
                PIXEL12_0
                PIXEL13_0
        } else {
                PIXEL00_12
                PIXEL01_14
                PIXEL02_83
                PIXEL03_50
                PIXEL12_70
                PIXEL13_21
        }
        PIXEL10_81
        PIXEL11_31
        PIXEL20_60
        PIXEL21_70
        PIXEL22_30
        PIXEL23_10
        PIXEL30_20
        PIXEL31_60
        PIXEL32_61
        PIXEL33_80
        

}

}

}

}else{
if(pattern<60){
if(pattern<58){
if(pattern<57){

        PIXEL00_80
        PIXEL01_61
        PIXEL02_61
        PIXEL03_80
        PIXEL10_10
        PIXEL11_30
        PIXEL12_30
        PIXEL13_10
        PIXEL20_31
        PIXEL21_31
        PIXEL22_30
        PIXEL23_10
        PIXEL30_81
        PIXEL31_81
        PIXEL32_61
        PIXEL33_80
        

}else{

        PIXEL00_82
        PIXEL01_82
        PIXEL02_61
        PIXEL03_80
        PIXEL10_32
        PIXEL11_32
        PIXEL12_30
        PIXEL13_10
        PIXEL20_31
        PIXEL21_31
        PIXEL22_30
        PIXEL23_10
        PIXEL30_81
        PIXEL31_81
        PIXEL32_61
        PIXEL33_80
        

}

}else{
if(pattern<59){

        if (Diff(yy[4], yy[2])) {
                PIXEL00_80
                PIXEL01_10
                PIXEL10_10
                PIXEL11_30
        } else {
                PIXEL00_20
                PIXEL01_12
                PIXEL10_11
                PIXEL11_0
        }
        if (Diff(yy[2], yy[6])) {
                PIXEL02_10
                PIXEL03_80
                PIXEL12_30
                PIXEL13_10
        } else {
                PIXEL02_11
                PIXEL03_20
                PIXEL12_0
                PIXEL13_12
        }
        PIXEL20_31
        PIXEL21_31
        PIXEL22_30
        PIXEL23_10
        PIXEL30_81
        PIXEL31_81
        PIXEL32_61
        PIXEL33_80
        

}else{

        if (Diff(yy[4], yy[2])) {
                PIXEL00_0
                PIXEL01_0
                PIXEL10_0
        } else {
                PIXEL00_50
                PIXEL01_50
                PIXEL10_50
        }
        if (Diff(yy[2], yy[6])) {
                PIXEL02_10
                PIXEL03_80
                PIXEL12_30
                PIXEL13_10
        } else {
                PIXEL02_11
                PIXEL03_20
                PIXEL12_0
                PIXEL13_12
        }
        PIXEL11_0
        PIXEL20_31
        PIXEL21_31
        PIXEL22_30
        PIXEL23_10
        PIXEL30_81
        PIXEL31_81
        PIXEL32_61
        PIXEL33_80
        

}

}

}else{
if(pattern<62){
if(pattern<61){

        PIXEL00_80
        PIXEL01_61
        PIXEL02_81
        PIXEL03_81
        PIXEL10_10
        PIXEL11_30
        PIXEL12_31
        PIXEL13_31
        PIXEL20_31
        PIXEL21_31
        PIXEL22_30
        PIXEL23_10
        PIXEL30_81
        PIXEL31_81
        PIXEL32_61
        PIXEL33_80
        

}else{

        PIXEL00_82
        PIXEL01_82
        PIXEL02_81
        PIXEL03_81
        PIXEL10_32
        PIXEL11_32
        PIXEL12_31
        PIXEL13_31
        PIXEL20_31
        PIXEL21_31
        PIXEL22_30
        PIXEL23_10
        PIXEL30_81
        PIXEL31_81
        PIXEL32_61
        PIXEL33_80
        

}

}else{
if(pattern<63){

        PIXEL00_80
        PIXEL01_10
        if (Diff(yy[2], yy[6])) {
                PIXEL02_0
                PIXEL03_0
                PIXEL13_0
        } else {
                PIXEL02_50
                PIXEL03_50
                PIXEL13_50
        }
        PIXEL10_10
        PIXEL11_30
        PIXEL12_0
        PIXEL20_31
        PIXEL21_31
        PIXEL22_30
        PIXEL23_10
        PIXEL30_81
        PIXEL31_81
        PIXEL32_61
        PIXEL33_80
        

}else{

        if (Diff(yy[4], yy[2])) {
                PIXEL00_0
        } else {
                PIXEL00_20
        }
        PIXEL01_0
        if (Diff(yy[2], yy[6])) {
                PIXEL02_0
                PIXEL03_0
                PIXEL13_0
        } else {
                PIXEL02_50
                PIXEL03_50
                PIXEL13_50
        }
        PIXEL10_0
        PIXEL11_0
        PIXEL12_0
        PIXEL20_31
        PIXEL21_31
        PIXEL22_30
        PIXEL23_10
        PIXEL30_81
        PIXEL31_81
        PIXEL32_61
        PIXEL33_80
        

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

        PIXEL00_20
        PIXEL01_60
        PIXEL02_60
        PIXEL03_20
        PIXEL10_60
        PIXEL11_70
        PIXEL12_70
        PIXEL13_60
        PIXEL20_61
        PIXEL21_30
        PIXEL22_30
        PIXEL23_61
        PIXEL30_80
        PIXEL31_10
        PIXEL32_10
        PIXEL33_80
        

}else{

        PIXEL00_20
        PIXEL01_60
        PIXEL02_60
        PIXEL03_20
        PIXEL10_60
        PIXEL11_70
        PIXEL12_70
        PIXEL13_60
        PIXEL20_61
        PIXEL21_30
        PIXEL22_30
        PIXEL23_61
        PIXEL30_80
        PIXEL31_10
        PIXEL32_10
        PIXEL33_80
        

}

}else{
if(pattern<67){

        PIXEL00_80
        PIXEL01_10
        PIXEL02_10
        PIXEL03_80
        PIXEL10_61
        PIXEL11_30
        PIXEL12_30
        PIXEL13_61
        PIXEL20_61
        PIXEL21_30
        PIXEL22_30
        PIXEL23_61
        PIXEL30_80
        PIXEL31_10
        PIXEL32_10
        PIXEL33_80
        

}else{

        PIXEL00_81
        PIXEL01_31
        PIXEL02_10
        PIXEL03_80
        PIXEL10_81
        PIXEL11_31
        PIXEL12_30
        PIXEL13_61
        PIXEL20_61
        PIXEL21_30
        PIXEL22_30
        PIXEL23_61
        PIXEL30_80
        PIXEL31_10
        PIXEL32_10
        PIXEL33_80
        

}

}

}else{
if(pattern<70){
if(pattern<69){

        PIXEL00_20
        PIXEL01_60
        PIXEL02_60
        PIXEL03_20
        PIXEL10_60
        PIXEL11_70
        PIXEL12_70
        PIXEL13_60
        PIXEL20_61
        PIXEL21_30
        PIXEL22_30
        PIXEL23_61
        PIXEL30_80
        PIXEL31_10
        PIXEL32_10
        PIXEL33_80
        

}else{

        PIXEL00_20
        PIXEL01_60
        PIXEL02_60
        PIXEL03_20
        PIXEL10_60
        PIXEL11_70
        PIXEL12_70
        PIXEL13_60
        PIXEL20_61
        PIXEL21_30
        PIXEL22_30
        PIXEL23_61
        PIXEL30_80
        PIXEL31_10
        PIXEL32_10
        PIXEL33_80
        

}

}else{
if(pattern<71){

        PIXEL00_80
        PIXEL01_10
        PIXEL02_32
        PIXEL03_82
        PIXEL10_61
        PIXEL11_30
        PIXEL12_32
        PIXEL13_82
        PIXEL20_61
        PIXEL21_30
        PIXEL22_30
        PIXEL23_61
        PIXEL30_80
        PIXEL31_10
        PIXEL32_10
        PIXEL33_80
        

}else{

        PIXEL00_81
        PIXEL01_31
        PIXEL02_32
        PIXEL03_82
        PIXEL10_81
        PIXEL11_31
        PIXEL12_32
        PIXEL13_82
        PIXEL20_61
        PIXEL21_30
        PIXEL22_30
        PIXEL23_61
        PIXEL30_80
        PIXEL31_10
        PIXEL32_10
        PIXEL33_80
        

}

}

}

}else{
if(pattern<76){
if(pattern<74){
if(pattern<73){

        PIXEL00_80
        PIXEL01_61
        PIXEL02_60
        PIXEL03_20
        PIXEL10_10
        PIXEL11_30
        PIXEL12_70
        PIXEL13_60
        if (Diff(yy[8], yy[4])) {
                PIXEL20_10
                PIXEL21_30
                PIXEL30_80
                PIXEL31_10
        } else {
                PIXEL20_50
                PIXEL21_0
                PIXEL30_50
                PIXEL31_50
        }
        PIXEL22_30
        PIXEL23_61
        PIXEL32_10
        PIXEL33_80
        

}else{

        if (Diff(yy[8], yy[4])) {
                PIXEL00_82
                PIXEL10_32
                PIXEL20_10
                PIXEL21_30
                PIXEL30_80
                PIXEL31_10
        } else {
                PIXEL00_11
                PIXEL10_13
                PIXEL20_83
                PIXEL21_70
                PIXEL30_50
                PIXEL31_21
        }
        PIXEL01_82
        PIXEL02_60
        PIXEL03_20
        PIXEL11_32
        PIXEL12_70
        PIXEL13_60
        PIXEL22_30
        PIXEL23_61
        PIXEL32_10
        PIXEL33_80
        

}

}else{
if(pattern<75){

        if (Diff(yy[4], yy[2])) {
                PIXEL00_0
                PIXEL01_0
                PIXEL10_0
        } else {
                PIXEL00_50
                PIXEL01_50
                PIXEL10_50
        }
        PIXEL02_10
        PIXEL03_80
        PIXEL11_0
        PIXEL12_30
        PIXEL13_61
        if (Diff(yy[8], yy[4])) {
                PIXEL20_0
                PIXEL30_0
                PIXEL31_0
        } else {
                PIXEL20_50
                PIXEL30_50
                PIXEL31_50
        }
        PIXEL21_0
        PIXEL22_30
        PIXEL23_61
        PIXEL32_10
        PIXEL33_80
        

}else{

        if (Diff(yy[4], yy[2])) {
                PIXEL00_0
                PIXEL01_0
                PIXEL10_0
        } else {
                PIXEL00_50
                PIXEL01_50
                PIXEL10_50
        }
        PIXEL02_10
        PIXEL03_80
        PIXEL11_0
        PIXEL12_30
        PIXEL13_61
        PIXEL20_10
        PIXEL21_30
        PIXEL22_30
        PIXEL23_61
        PIXEL30_80
        PIXEL31_10
        PIXEL32_10
        PIXEL33_80
        

}

}

}else{
if(pattern<78){
if(pattern<77){

        PIXEL00_80
        PIXEL01_61
        PIXEL02_60
        PIXEL03_20
        PIXEL10_10
        PIXEL11_30
        PIXEL12_70
        PIXEL13_60
        if (Diff(yy[8], yy[4])) {
                PIXEL20_10
                PIXEL21_30
                PIXEL30_80
                PIXEL31_10
        } else {
                PIXEL20_50
                PIXEL21_0
                PIXEL30_50
                PIXEL31_50
        }
        PIXEL22_30
        PIXEL23_61
        PIXEL32_10
        PIXEL33_80
        

}else{

        if (Diff(yy[8], yy[4])) {
                PIXEL00_82
                PIXEL10_32
                PIXEL20_10
                PIXEL21_30
                PIXEL30_80
                PIXEL31_10
        } else {
                PIXEL00_11
                PIXEL10_13
                PIXEL20_83
                PIXEL21_70
                PIXEL30_50
                PIXEL31_21
        }
        PIXEL01_82
        PIXEL02_60
        PIXEL03_20
        PIXEL11_32
        PIXEL12_70
        PIXEL13_60
        PIXEL22_30
        PIXEL23_61
        PIXEL32_10
        PIXEL33_80
        

}

}else{
if(pattern<79){

        if (Diff(yy[4], yy[2])) {
                PIXEL00_80
                PIXEL01_10
                PIXEL10_10
                PIXEL11_30
        } else {
                PIXEL00_20
                PIXEL01_12
                PIXEL10_11
                PIXEL11_0
        }
        PIXEL02_32
        PIXEL03_82
        PIXEL12_32
        PIXEL13_82
        if (Diff(yy[8], yy[4])) {
                PIXEL20_10
                PIXEL21_30
                PIXEL30_80
                PIXEL31_10
        } else {
                PIXEL20_12
                PIXEL21_0
                PIXEL30_20
                PIXEL31_11
        }
        PIXEL22_30
        PIXEL23_61
        PIXEL32_10
        PIXEL33_80
        

}else{

        if (Diff(yy[4], yy[2])) {
                PIXEL00_0
                PIXEL01_0
                PIXEL10_0
        } else {
                PIXEL00_50
                PIXEL01_50
                PIXEL10_50
        }
        PIXEL02_32
        PIXEL03_82
        PIXEL11_0
        PIXEL12_32
        PIXEL13_82
        if (Diff(yy[8], yy[4])) {
                PIXEL20_10
                PIXEL21_30
                PIXEL30_80
                PIXEL31_10
        } else {
                PIXEL20_12
                PIXEL21_0
                PIXEL30_20
                PIXEL31_11
        }
        PIXEL22_30
        PIXEL23_61
        PIXEL32_10
        PIXEL33_80
        

}

}

}

}

}else{
if(pattern<88){
if(pattern<84){
if(pattern<82){
if(pattern<81){

        PIXEL00_20
        PIXEL01_60
        PIXEL02_61
        PIXEL03_80
        PIXEL10_60
        PIXEL11_70
        PIXEL12_30
        PIXEL13_10
        PIXEL20_61
        PIXEL21_30
        if (Diff(yy[6], yy[8])) {
                PIXEL22_30
                PIXEL23_10
                PIXEL32_10
                PIXEL33_80
        } else {
                PIXEL22_0
                PIXEL23_50
                PIXEL32_50
                PIXEL33_50
        }
        PIXEL30_80
        PIXEL31_10
        

}else{

        PIXEL00_20
        PIXEL01_60
        PIXEL02_61
        PIXEL03_80
        PIXEL10_60
        PIXEL11_70
        PIXEL12_30
        PIXEL13_10
        PIXEL20_61
        PIXEL21_30
        if (Diff(yy[6], yy[8])) {
                PIXEL22_30
                PIXEL23_10
                PIXEL32_10
                PIXEL33_80
        } else {
                PIXEL22_0
                PIXEL23_50
                PIXEL32_50
                PIXEL33_50
        }
        PIXEL30_80
        PIXEL31_10
        

}

}else{
if(pattern<83){

        PIXEL00_80
        PIXEL01_10
        if (Diff(yy[2], yy[6])) {
                PIXEL02_0
                PIXEL03_0
                PIXEL13_0
        } else {
                PIXEL02_50
                PIXEL03_50
                PIXEL13_50
        }
        PIXEL10_61
        PIXEL11_30
        PIXEL12_0
        PIXEL20_61
        PIXEL21_30
        PIXEL22_0
        if (Diff(yy[6], yy[8])) {
                PIXEL23_0
                PIXEL32_0
                PIXEL33_0
        } else {
                PIXEL23_50
                PIXEL32_50
                PIXEL33_50
        }
        PIXEL30_80
        PIXEL31_10
        

}else{

        PIXEL00_81
        PIXEL01_31
        if (Diff(yy[2], yy[6])) {
                PIXEL02_10
                PIXEL03_80
                PIXEL12_30
                PIXEL13_10
        } else {
                PIXEL02_11
                PIXEL03_20
                PIXEL12_0
                PIXEL13_12
        }
        PIXEL10_81
        PIXEL11_31
        PIXEL20_61
        PIXEL21_30
        if (Diff(yy[6], yy[8])) {
                PIXEL22_30
                PIXEL23_10
                PIXEL32_10
                PIXEL33_80
        } else {
                PIXEL22_0
                PIXEL23_11
                PIXEL32_12
                PIXEL33_20
        }
        PIXEL30_80
        PIXEL31_10
        

}

}

}else{
if(pattern<86){
if(pattern<85){

        PIXEL00_20
        PIXEL01_60
        PIXEL02_81
        if (Diff(yy[6], yy[8])) {
                PIXEL03_81
                PIXEL13_31
                PIXEL22_30
                PIXEL23_10
                PIXEL32_10
                PIXEL33_80
        } else {
                PIXEL03_12
                PIXEL13_14
                PIXEL22_70
                PIXEL23_83
                PIXEL32_21
                PIXEL33_50
        }
        PIXEL10_60
        PIXEL11_70
        PIXEL12_31
        PIXEL20_61
        PIXEL21_30
        PIXEL30_80
        PIXEL31_10
        

}else{

        PIXEL00_20
        PIXEL01_60
        PIXEL02_81
        if (Diff(yy[6], yy[8])) {
                PIXEL03_81
                PIXEL13_31
                PIXEL22_30
                PIXEL23_10
                PIXEL32_10
                PIXEL33_80
        } else {
                PIXEL03_12
                PIXEL13_14
                PIXEL22_70
                PIXEL23_83
                PIXEL32_21
                PIXEL33_50
        }
        PIXEL10_60
        PIXEL11_70
        PIXEL12_31
        PIXEL20_61
        PIXEL21_30
        PIXEL30_80
        PIXEL31_10
        

}

}else{
if(pattern<87){

        PIXEL00_80
        PIXEL01_10
        if (Diff(yy[2], yy[6])) {
                PIXEL02_0
                PIXEL03_0
                PIXEL13_0
        } else {
                PIXEL02_50
                PIXEL03_50
                PIXEL13_50
        }
        PIXEL10_61
        PIXEL11_30
        PIXEL12_0
        PIXEL20_61
        PIXEL21_30
        PIXEL22_30
        PIXEL23_10
        PIXEL30_80
        PIXEL31_10
        PIXEL32_10
        PIXEL33_80
        

}else{

        PIXEL00_81
        PIXEL01_31
        if (Diff(yy[2], yy[6])) {
                PIXEL02_0
                PIXEL03_0
                PIXEL13_0
        } else {
                PIXEL02_50
                PIXEL03_50
                PIXEL13_50
        }
        PIXEL10_81
        PIXEL11_31
        PIXEL12_0
        PIXEL20_61
        PIXEL21_30
        if (Diff(yy[6], yy[8])) {
                PIXEL22_30
                PIXEL23_10
                PIXEL32_10
                PIXEL33_80
        } else {
                PIXEL22_0
                PIXEL23_11
                PIXEL32_12
                PIXEL33_20
        }
        PIXEL30_80
        PIXEL31_10
        

}

}

}

}else{
if(pattern<92){
if(pattern<90){
if(pattern<89){

        PIXEL00_80
        PIXEL01_61
        PIXEL02_61
        PIXEL03_80
        PIXEL10_10
        PIXEL11_30
        PIXEL12_30
        PIXEL13_10
        if (Diff(yy[8], yy[4])) {
                PIXEL20_0
                PIXEL30_0
                PIXEL31_0
        } else {
                PIXEL20_50
                PIXEL30_50
                PIXEL31_50
        }
        PIXEL21_0
        PIXEL22_0
        if (Diff(yy[6], yy[8])) {
                PIXEL23_0
                PIXEL32_0
                PIXEL33_0
        } else {
                PIXEL23_50
                PIXEL32_50
                PIXEL33_50
        }
        

}else{

        PIXEL00_82
        PIXEL01_82
        PIXEL02_61
        PIXEL03_80
        PIXEL10_32
        PIXEL11_32
        PIXEL12_30
        PIXEL13_10
        if (Diff(yy[8], yy[4])) {
                PIXEL20_10
                PIXEL21_30
                PIXEL30_80
                PIXEL31_10
        } else {
                PIXEL20_12
                PIXEL21_0
                PIXEL30_20
                PIXEL31_11
        }
        if (Diff(yy[6], yy[8])) {
                PIXEL22_30
                PIXEL23_10
                PIXEL32_10
                PIXEL33_80
        } else {
                PIXEL22_0
                PIXEL23_11
                PIXEL32_12
                PIXEL33_20
        }
        

}

}else{
if(pattern<91){

        if (Diff(yy[4], yy[2])) {
                PIXEL00_80
                PIXEL01_10
                PIXEL10_10
                PIXEL11_30
        } else {
                PIXEL00_20
                PIXEL01_12
                PIXEL10_11
                PIXEL11_0
        }
        if (Diff(yy[2], yy[6])) {
                PIXEL02_10
                PIXEL03_80
                PIXEL12_30
                PIXEL13_10
        } else {
                PIXEL02_11
                PIXEL03_20
                PIXEL12_0
                PIXEL13_12
        }
        if (Diff(yy[8], yy[4])) {
                PIXEL20_10
                PIXEL21_30
                PIXEL30_80
                PIXEL31_10
        } else {
                PIXEL20_12
                PIXEL21_0
                PIXEL30_20
                PIXEL31_11
        }
        if (Diff(yy[6], yy[8])) {
                PIXEL22_30
                PIXEL23_10
                PIXEL32_10
                PIXEL33_80
        } else {
                PIXEL22_0
                PIXEL23_11
                PIXEL32_12
                PIXEL33_20
        }
        

}else{

        if (Diff(yy[4], yy[2])) {
                PIXEL00_0
                PIXEL01_0
                PIXEL10_0
        } else {
                PIXEL00_50
                PIXEL01_50
                PIXEL10_50
        }
        if (Diff(yy[2], yy[6])) {
                PIXEL02_10
                PIXEL03_80
                PIXEL12_30
                PIXEL13_10
        } else {
                PIXEL02_11
                PIXEL03_20
                PIXEL12_0
                PIXEL13_12
        }
        PIXEL11_0
        if (Diff(yy[8], yy[4])) {
                PIXEL20_10
                PIXEL21_30
                PIXEL30_80
                PIXEL31_10
        } else {
                PIXEL20_12
                PIXEL21_0
                PIXEL30_20
                PIXEL31_11
        }
        if (Diff(yy[6], yy[8])) {
                PIXEL22_30
                PIXEL23_10
                PIXEL32_10
                PIXEL33_80
        } else {
                PIXEL22_0
                PIXEL23_11
                PIXEL32_12
                PIXEL33_20
        }
        

}

}

}else{
if(pattern<94){
if(pattern<93){

        PIXEL00_80
        PIXEL01_61
        PIXEL02_81
        PIXEL03_81
        PIXEL10_10
        PIXEL11_30
        PIXEL12_31
        PIXEL13_31
        if (Diff(yy[8], yy[4])) {
                PIXEL20_10
                PIXEL21_30
                PIXEL30_80
                PIXEL31_10
        } else {
                PIXEL20_12
                PIXEL21_0
                PIXEL30_20
                PIXEL31_11
        }
        if (Diff(yy[6], yy[8])) {
                PIXEL22_30
                PIXEL23_10
                PIXEL32_10
                PIXEL33_80
        } else {
                PIXEL22_0
                PIXEL23_11
                PIXEL32_12
                PIXEL33_20
        }
        

}else{

        PIXEL00_82
        PIXEL01_82
        PIXEL02_81
        PIXEL03_81
        PIXEL10_32
        PIXEL11_32
        PIXEL12_31
        PIXEL13_31
        if (Diff(yy[8], yy[4])) {
                PIXEL20_10
                PIXEL21_30
                PIXEL30_80
                PIXEL31_10
        } else {
                PIXEL20_12
                PIXEL21_0
                PIXEL30_20
                PIXEL31_11
        }
        if (Diff(yy[6], yy[8])) {
                PIXEL22_30
                PIXEL23_10
                PIXEL32_10
                PIXEL33_80
        } else {
                PIXEL22_0
                PIXEL23_11
                PIXEL32_12
                PIXEL33_20
        }
        

}

}else{
if(pattern<95){

        if (Diff(yy[4], yy[2])) {
                PIXEL00_80
                PIXEL01_10
                PIXEL10_10
                PIXEL11_30
        } else {
                PIXEL00_20
                PIXEL01_12
                PIXEL10_11
                PIXEL11_0
        }
        if (Diff(yy[2], yy[6])) {
                PIXEL02_0
                PIXEL03_0
                PIXEL13_0
        } else {
                PIXEL02_50
                PIXEL03_50
                PIXEL13_50
        }
        PIXEL12_0
        if (Diff(yy[8], yy[4])) {
                PIXEL20_10
                PIXEL21_30
                PIXEL30_80
                PIXEL31_10
        } else {
                PIXEL20_12
                PIXEL21_0
                PIXEL30_20
                PIXEL31_11
        }
        if (Diff(yy[6], yy[8])) {
                PIXEL22_30
                PIXEL23_10
                PIXEL32_10
                PIXEL33_80
        } else {
                PIXEL22_0
                PIXEL23_11
                PIXEL32_12
                PIXEL33_20
        }
        

}else{

        if (Diff(yy[4], yy[2])) {
                PIXEL00_0
                PIXEL01_0
                PIXEL10_0
        } else {
                PIXEL00_50
                PIXEL01_50
                PIXEL10_50
        }
        if (Diff(yy[2], yy[6])) {
                PIXEL02_0
                PIXEL03_0
                PIXEL13_0
        } else {
                PIXEL02_50
                PIXEL03_50
                PIXEL13_50
        }
        PIXEL11_0
        PIXEL12_0
        PIXEL20_10
        PIXEL21_30
        PIXEL22_30
        PIXEL23_10
        PIXEL30_80
        PIXEL31_10
        PIXEL32_10
        PIXEL33_80
        

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

        PIXEL00_20
        PIXEL01_60
        PIXEL02_60
        PIXEL03_20
        PIXEL10_60
        PIXEL11_70
        PIXEL12_70
        PIXEL13_60
        PIXEL20_82
        PIXEL21_32
        PIXEL22_30
        PIXEL23_61
        PIXEL30_82
        PIXEL31_32
        PIXEL32_10
        PIXEL33_80
        

}else{

        PIXEL00_20
        PIXEL01_60
        PIXEL02_60
        PIXEL03_20
        PIXEL10_60
        PIXEL11_70
        PIXEL12_70
        PIXEL13_60
        PIXEL20_82
        PIXEL21_32
        PIXEL22_30
        PIXEL23_61
        PIXEL30_82
        PIXEL31_32
        PIXEL32_10
        PIXEL33_80
        

}

}else{
if(pattern<99){

        PIXEL00_80
        PIXEL01_10
        PIXEL02_10
        PIXEL03_80
        PIXEL10_61
        PIXEL11_30
        PIXEL12_30
        PIXEL13_61
        PIXEL20_82
        PIXEL21_32
        PIXEL22_30
        PIXEL23_61
        PIXEL30_82
        PIXEL31_32
        PIXEL32_10
        PIXEL33_80
        

}else{

        PIXEL00_81
        PIXEL01_31
        PIXEL02_10
        PIXEL03_80
        PIXEL10_81
        PIXEL11_31
        PIXEL12_30
        PIXEL13_61
        PIXEL20_82
        PIXEL21_32
        PIXEL22_30
        PIXEL23_61
        PIXEL30_82
        PIXEL31_32
        PIXEL32_10
        PIXEL33_80
        

}

}

}else{
if(pattern<102){
if(pattern<101){

        PIXEL00_20
        PIXEL01_60
        PIXEL02_60
        PIXEL03_20
        PIXEL10_60
        PIXEL11_70
        PIXEL12_70
        PIXEL13_60
        PIXEL20_82
        PIXEL21_32
        PIXEL22_30
        PIXEL23_61
        PIXEL30_82
        PIXEL31_32
        PIXEL32_10
        PIXEL33_80
        

}else{

        PIXEL00_20
        PIXEL01_60
        PIXEL02_60
        PIXEL03_20
        PIXEL10_60
        PIXEL11_70
        PIXEL12_70
        PIXEL13_60
        PIXEL20_82
        PIXEL21_32
        PIXEL22_30
        PIXEL23_61
        PIXEL30_82
        PIXEL31_32
        PIXEL32_10
        PIXEL33_80
        

}

}else{
if(pattern<103){

        PIXEL00_80
        PIXEL01_10
        PIXEL02_32
        PIXEL03_82
        PIXEL10_61
        PIXEL11_30
        PIXEL12_32
        PIXEL13_82
        PIXEL20_82
        PIXEL21_32
        PIXEL22_30
        PIXEL23_61
        PIXEL30_82
        PIXEL31_32
        PIXEL32_10
        PIXEL33_80
        

}else{

        PIXEL00_81
        PIXEL01_31
        PIXEL02_32
        PIXEL03_82
        PIXEL10_81
        PIXEL11_31
        PIXEL12_32
        PIXEL13_82
        PIXEL20_82
        PIXEL21_32
        PIXEL22_30
        PIXEL23_61
        PIXEL30_82
        PIXEL31_32
        PIXEL32_10
        PIXEL33_80
        

}

}

}

}else{
if(pattern<108){
if(pattern<106){
if(pattern<105){

        PIXEL00_80
        PIXEL01_61
        PIXEL02_60
        PIXEL03_20
        PIXEL10_10
        PIXEL11_30
        PIXEL12_70
        PIXEL13_60
        if (Diff(yy[8], yy[4])) {
                PIXEL20_0
                PIXEL30_0
                PIXEL31_0
        } else {
                PIXEL20_50
                PIXEL30_50
                PIXEL31_50
        }
        PIXEL21_0
        PIXEL22_30
        PIXEL23_61
        PIXEL32_10
        PIXEL33_80
        

}else{

        if (Diff(yy[8], yy[4])) {
                PIXEL00_82
                PIXEL10_32
                PIXEL20_0
                PIXEL21_0
                PIXEL30_0
                PIXEL31_0
        } else {
                PIXEL00_11
                PIXEL10_13
                PIXEL20_83
                PIXEL21_70
                PIXEL30_50
                PIXEL31_21
        }
        PIXEL01_82
        PIXEL02_60
        PIXEL03_20
        PIXEL11_32
        PIXEL12_70
        PIXEL13_60
        PIXEL22_30
        PIXEL23_61
        PIXEL32_10
        PIXEL33_80
        

}

}else{
if(pattern<107){

        PIXEL00_80
        PIXEL01_10
        PIXEL02_10
        PIXEL03_80
        PIXEL10_10
        PIXEL11_30
        PIXEL12_30
        PIXEL13_61
        if (Diff(yy[8], yy[4])) {
                PIXEL20_0
                PIXEL30_0
                PIXEL31_0
        } else {
                PIXEL20_50
                PIXEL30_50
                PIXEL31_50
        }
        PIXEL21_0
        PIXEL22_30
        PIXEL23_61
        PIXEL32_10
        PIXEL33_80
        

}else{

        if (Diff(yy[4], yy[2])) {
                PIXEL00_0
                PIXEL01_0
                PIXEL10_0
        } else {
                PIXEL00_50
                PIXEL01_50
                PIXEL10_50
        }
        PIXEL02_10
        PIXEL03_80
        PIXEL11_0
        PIXEL12_30
        PIXEL13_61
        if (Diff(yy[8], yy[4])) {
                PIXEL20_0
                PIXEL30_0
                PIXEL31_0
        } else {
                PIXEL20_50
                PIXEL30_50
                PIXEL31_50
        }
        PIXEL21_0
        PIXEL22_30
        PIXEL23_61
        PIXEL32_10
        PIXEL33_80
        

}

}

}else{
if(pattern<110){
if(pattern<109){

        PIXEL00_80
        PIXEL01_61
        PIXEL02_60
        PIXEL03_20
        PIXEL10_10
        PIXEL11_30
        PIXEL12_70
        PIXEL13_60
        if (Diff(yy[8], yy[4])) {
                PIXEL20_0
                PIXEL30_0
                PIXEL31_0
        } else {
                PIXEL20_50
                PIXEL30_50
                PIXEL31_50
        }
        PIXEL21_0
        PIXEL22_30
        PIXEL23_61
        PIXEL32_10
        PIXEL33_80
        

}else{

        if (Diff(yy[8], yy[4])) {
                PIXEL00_82
                PIXEL10_32
                PIXEL20_0
                PIXEL21_0
                PIXEL30_0
                PIXEL31_0
        } else {
                PIXEL00_11
                PIXEL10_13
                PIXEL20_83
                PIXEL21_70
                PIXEL30_50
                PIXEL31_21
        }
        PIXEL01_82
        PIXEL02_60
        PIXEL03_20
        PIXEL11_32
        PIXEL12_70
        PIXEL13_60
        PIXEL22_30
        PIXEL23_61
        PIXEL32_10
        PIXEL33_80
        

}

}else{
if(pattern<111){

        PIXEL00_80
        PIXEL01_10
        PIXEL02_32
        PIXEL03_82
        PIXEL10_10
        PIXEL11_30
        PIXEL12_32
        PIXEL13_82
        if (Diff(yy[8], yy[4])) {
                PIXEL20_0
                PIXEL30_0
                PIXEL31_0
        } else {
                PIXEL20_50
                PIXEL30_50
                PIXEL31_50
        }
        PIXEL21_0
        PIXEL22_30
        PIXEL23_61
        PIXEL32_10
        PIXEL33_80
        

}else{

        if (Diff(yy[4], yy[2])) {
                PIXEL00_0
        } else {
                PIXEL00_20
        }
        PIXEL01_0
        PIXEL02_32
        PIXEL03_82
        PIXEL10_0
        PIXEL11_0
        PIXEL12_32
        PIXEL13_82
        if (Diff(yy[8], yy[4])) {
                PIXEL20_0
                PIXEL30_0
                PIXEL31_0
        } else {
                PIXEL20_50
                PIXEL30_50
                PIXEL31_50
        }
        PIXEL21_0
        PIXEL22_30
        PIXEL23_61
        PIXEL32_10
        PIXEL33_80
        

}

}

}

}

}else{
if(pattern<120){
if(pattern<116){
if(pattern<114){
if(pattern<113){

        PIXEL00_20
        PIXEL01_60
        PIXEL02_61
        PIXEL03_80
        PIXEL10_60
        PIXEL11_70
        PIXEL12_30
        PIXEL13_10
        PIXEL20_82
        PIXEL21_32
        if (Diff(yy[6], yy[8])) {
                PIXEL22_30
                PIXEL23_10
                PIXEL30_82
                PIXEL31_32
                PIXEL32_10
                PIXEL33_80
        } else {
                PIXEL22_70
                PIXEL23_21
                PIXEL30_11
                PIXEL31_13
                PIXEL32_83
                PIXEL33_50
        }
        

}else{

        PIXEL00_20
        PIXEL01_60
        PIXEL02_61
        PIXEL03_80
        PIXEL10_60
        PIXEL11_70
        PIXEL12_30
        PIXEL13_10
        PIXEL20_82
        PIXEL21_32
        if (Diff(yy[6], yy[8])) {
                PIXEL22_30
                PIXEL23_10
                PIXEL30_82
                PIXEL31_32
                PIXEL32_10
                PIXEL33_80
        } else {
                PIXEL22_70
                PIXEL23_21
                PIXEL30_11
                PIXEL31_13
                PIXEL32_83
                PIXEL33_50
        }
        

}

}else{
if(pattern<115){

        PIXEL00_80
        PIXEL01_10
        if (Diff(yy[2], yy[6])) {
                PIXEL02_10
                PIXEL03_80
                PIXEL12_30
                PIXEL13_10
        } else {
                PIXEL02_11
                PIXEL03_20
                PIXEL12_0
                PIXEL13_12
        }
        PIXEL10_61
        PIXEL11_30
        PIXEL20_82
        PIXEL21_32
        if (Diff(yy[6], yy[8])) {
                PIXEL22_30
                PIXEL23_10
                PIXEL32_10
                PIXEL33_80
        } else {
                PIXEL22_0
                PIXEL23_11
                PIXEL32_12
                PIXEL33_20
        }
        PIXEL30_82
        PIXEL31_32
        

}else{

        PIXEL00_81
        PIXEL01_31
        if (Diff(yy[2], yy[6])) {
                PIXEL02_10
                PIXEL03_80
                PIXEL12_30
                PIXEL13_10
        } else {
                PIXEL02_11
                PIXEL03_20
                PIXEL12_0
                PIXEL13_12
        }
        PIXEL10_81
        PIXEL11_31
        PIXEL20_82
        PIXEL21_32
        if (Diff(yy[6], yy[8])) {
                PIXEL22_30
                PIXEL23_10
                PIXEL32_10
                PIXEL33_80
        } else {
                PIXEL22_0
                PIXEL23_11
                PIXEL32_12
                PIXEL33_20
        }
        PIXEL30_82
        PIXEL31_32
        

}

}

}else{
if(pattern<118){
if(pattern<117){

        PIXEL00_20
        PIXEL01_60
        PIXEL02_81
        PIXEL03_81
        PIXEL10_60
        PIXEL11_70
        PIXEL12_31
        PIXEL13_31
        PIXEL20_82
        PIXEL21_32
        if (Diff(yy[6], yy[8])) {
                PIXEL22_30
                PIXEL23_10
                PIXEL32_10
                PIXEL33_80
        } else {
                PIXEL22_0
                PIXEL23_11
                PIXEL32_12
                PIXEL33_20
        }
        PIXEL30_82
        PIXEL31_32
        

}else{

        PIXEL00_20
        PIXEL01_60
        PIXEL02_81
        PIXEL03_81
        PIXEL10_60
        PIXEL11_70
        PIXEL12_31
        PIXEL13_31
        PIXEL20_82
        PIXEL21_32
        if (Diff(yy[6], yy[8])) {
                PIXEL22_30
                PIXEL23_10
                PIXEL32_10
                PIXEL33_80
        } else {
                PIXEL22_0
                PIXEL23_11
                PIXEL32_12
                PIXEL33_20
        }
        PIXEL30_82
        PIXEL31_32
        

}

}else{
if(pattern<119){

        PIXEL00_80
        PIXEL01_10
        if (Diff(yy[2], yy[6])) {
                PIXEL02_0
                PIXEL03_0
                PIXEL13_0
        } else {
                PIXEL02_50
                PIXEL03_50
                PIXEL13_50
        }
        PIXEL10_61
        PIXEL11_30
        PIXEL12_0
        PIXEL20_82
        PIXEL21_32
        PIXEL22_30
        PIXEL23_10
        PIXEL30_82
        PIXEL31_32
        PIXEL32_10
        PIXEL33_80
        

}else{

        if (Diff(yy[2], yy[6])) {
                PIXEL00_81
                PIXEL01_31
                PIXEL02_0
                PIXEL03_0
                PIXEL12_0
                PIXEL13_0
        } else {
                PIXEL00_12
                PIXEL01_14
                PIXEL02_83
                PIXEL03_50
                PIXEL12_70
                PIXEL13_21
        }
        PIXEL10_81
        PIXEL11_31
        PIXEL20_82
        PIXEL21_32
        PIXEL22_30
        PIXEL23_10
        PIXEL30_82
        PIXEL31_32
        PIXEL32_10
        PIXEL33_80
        

}

}

}

}else{
if(pattern<124){
if(pattern<122){
if(pattern<121){

        PIXEL00_80
        PIXEL01_61
        PIXEL02_61
        PIXEL03_80
        PIXEL10_10
        PIXEL11_30
        PIXEL12_30
        PIXEL13_10
        if (Diff(yy[8], yy[4])) {
                PIXEL20_0
                PIXEL30_0
                PIXEL31_0
        } else {
                PIXEL20_50
                PIXEL30_50
                PIXEL31_50
        }
        PIXEL21_0
        PIXEL22_30
        PIXEL23_10
        PIXEL32_10
        PIXEL33_80
        

}else{

        PIXEL00_82
        PIXEL01_82
        PIXEL02_61
        PIXEL03_80
        PIXEL10_32
        PIXEL11_32
        PIXEL12_30
        PIXEL13_10
        if (Diff(yy[8], yy[4])) {
                PIXEL20_0
                PIXEL30_0
                PIXEL31_0
        } else {
                PIXEL20_50
                PIXEL30_50
                PIXEL31_50
        }
        PIXEL21_0
        if (Diff(yy[6], yy[8])) {
                PIXEL22_30
                PIXEL23_10
                PIXEL32_10
                PIXEL33_80
        } else {
                PIXEL22_0
                PIXEL23_11
                PIXEL32_12
                PIXEL33_20
        }
        

}

}else{
if(pattern<123){

        if (Diff(yy[4], yy[2])) {
                PIXEL00_80
                PIXEL01_10
                PIXEL10_10
                PIXEL11_30
        } else {
                PIXEL00_20
                PIXEL01_12
                PIXEL10_11
                PIXEL11_0
        }
        if (Diff(yy[2], yy[6])) {
                PIXEL02_10
                PIXEL03_80
                PIXEL12_30
                PIXEL13_10
        } else {
                PIXEL02_11
                PIXEL03_20
                PIXEL12_0
                PIXEL13_12
        }
        if (Diff(yy[8], yy[4])) {
                PIXEL20_0
                PIXEL30_0
                PIXEL31_0
        } else {
                PIXEL20_50
                PIXEL30_50
                PIXEL31_50
        }
        PIXEL21_0
        if (Diff(yy[6], yy[8])) {
                PIXEL22_30
                PIXEL23_10
                PIXEL32_10
                PIXEL33_80
        } else {
                PIXEL22_0
                PIXEL23_11
                PIXEL32_12
                PIXEL33_20
        }
        

}else{

        if (Diff(yy[4], yy[2])) {
                PIXEL00_0
                PIXEL01_0
                PIXEL10_0
        } else {
                PIXEL00_50
                PIXEL01_50
                PIXEL10_50
        }
        PIXEL02_10
        PIXEL03_80
        PIXEL11_0
        PIXEL12_30
        PIXEL13_10
        if (Diff(yy[8], yy[4])) {
                PIXEL20_0
                PIXEL30_0
                PIXEL31_0
        } else {
                PIXEL20_50
                PIXEL30_50
                PIXEL31_50
        }
        PIXEL21_0
        PIXEL22_30
        PIXEL23_10
        PIXEL32_10
        PIXEL33_80
        

}

}

}else{
if(pattern<126){
if(pattern<125){

        PIXEL00_80
        PIXEL01_61
        PIXEL02_81
        PIXEL03_81
        PIXEL10_10
        PIXEL11_30
        PIXEL12_31
        PIXEL13_31
        if (Diff(yy[8], yy[4])) {
                PIXEL20_0
                PIXEL30_0
                PIXEL31_0
        } else {
                PIXEL20_50
                PIXEL30_50
                PIXEL31_50
        }
        PIXEL21_0
        PIXEL22_30
        PIXEL23_10
        PIXEL32_10
        PIXEL33_80
        

}else{

        if (Diff(yy[8], yy[4])) {
                PIXEL00_82
                PIXEL10_32
                PIXEL20_0
                PIXEL21_0
                PIXEL30_0
                PIXEL31_0
        } else {
                PIXEL00_11
                PIXEL10_13
                PIXEL20_83
                PIXEL21_70
                PIXEL30_50
                PIXEL31_21
        }
        PIXEL01_82
        PIXEL02_81
        PIXEL03_81
        PIXEL11_32
        PIXEL12_31
        PIXEL13_31
        PIXEL22_30
        PIXEL23_10
        PIXEL32_10
        PIXEL33_80
        

}

}else{
if(pattern<127){

        PIXEL00_80
        PIXEL01_10
        if (Diff(yy[2], yy[6])) {
                PIXEL02_0
                PIXEL03_0
                PIXEL13_0
        } else {
                PIXEL02_50
                PIXEL03_50
                PIXEL13_50
        }
        PIXEL10_10
        PIXEL11_30
        PIXEL12_0
        if (Diff(yy[8], yy[4])) {
                PIXEL20_0
                PIXEL30_0
                PIXEL31_0
        } else {
                PIXEL20_50
                PIXEL30_50
                PIXEL31_50
        }
        PIXEL21_0
        PIXEL22_30
        PIXEL23_10
        PIXEL32_10
        PIXEL33_80
        

}else{

        if (Diff(yy[4], yy[2])) {
                PIXEL00_0
        } else {
                PIXEL00_20
        }
        PIXEL01_0
        if (Diff(yy[2], yy[6])) {
                PIXEL02_0
                PIXEL03_0
                PIXEL13_0
        } else {
                PIXEL02_50
                PIXEL03_50
                PIXEL13_50
        }
        PIXEL10_0
        PIXEL11_0
        PIXEL12_0
        if (Diff(yy[8], yy[4])) {
                PIXEL20_0
                PIXEL30_0
                PIXEL31_0
        } else {
                PIXEL20_50
                PIXEL30_50
                PIXEL31_50
        }
        PIXEL21_0
        PIXEL22_30
        PIXEL23_10
        PIXEL32_10
        PIXEL33_80
        

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

        PIXEL00_20
        PIXEL01_60
        PIXEL02_60
        PIXEL03_20
        PIXEL10_60
        PIXEL11_70
        PIXEL12_70
        PIXEL13_60
        PIXEL20_60
        PIXEL21_70
        PIXEL22_70
        PIXEL23_60
        PIXEL30_20
        PIXEL31_60
        PIXEL32_60
        PIXEL33_20
        

}else{

        PIXEL00_20
        PIXEL01_60
        PIXEL02_60
        PIXEL03_20
        PIXEL10_60
        PIXEL11_70
        PIXEL12_70
        PIXEL13_60
        PIXEL20_60
        PIXEL21_70
        PIXEL22_70
        PIXEL23_60
        PIXEL30_20
        PIXEL31_60
        PIXEL32_60
        PIXEL33_20
        

}

}else{
if(pattern<131){

        PIXEL00_80
        PIXEL01_10
        PIXEL02_10
        PIXEL03_80
        PIXEL10_61
        PIXEL11_30
        PIXEL12_30
        PIXEL13_61
        PIXEL20_60
        PIXEL21_70
        PIXEL22_70
        PIXEL23_60
        PIXEL30_20
        PIXEL31_60
        PIXEL32_60
        PIXEL33_20
        

}else{

        PIXEL00_81
        PIXEL01_31
        PIXEL02_10
        PIXEL03_80
        PIXEL10_81
        PIXEL11_31
        PIXEL12_30
        PIXEL13_61
        PIXEL20_60
        PIXEL21_70
        PIXEL22_70
        PIXEL23_60
        PIXEL30_20
        PIXEL31_60
        PIXEL32_60
        PIXEL33_20
        

}

}

}else{
if(pattern<134){
if(pattern<133){

        PIXEL00_20
        PIXEL01_60
        PIXEL02_60
        PIXEL03_20
        PIXEL10_60
        PIXEL11_70
        PIXEL12_70
        PIXEL13_60
        PIXEL20_60
        PIXEL21_70
        PIXEL22_70
        PIXEL23_60
        PIXEL30_20
        PIXEL31_60
        PIXEL32_60
        PIXEL33_20
        

}else{

        PIXEL00_20
        PIXEL01_60
        PIXEL02_60
        PIXEL03_20
        PIXEL10_60
        PIXEL11_70
        PIXEL12_70
        PIXEL13_60
        PIXEL20_60
        PIXEL21_70
        PIXEL22_70
        PIXEL23_60
        PIXEL30_20
        PIXEL31_60
        PIXEL32_60
        PIXEL33_20
        

}

}else{
if(pattern<135){

        PIXEL00_80
        PIXEL01_10
        PIXEL02_32
        PIXEL03_82
        PIXEL10_61
        PIXEL11_30
        PIXEL12_32
        PIXEL13_82
        PIXEL20_60
        PIXEL21_70
        PIXEL22_70
        PIXEL23_60
        PIXEL30_20
        PIXEL31_60
        PIXEL32_60
        PIXEL33_20
        

}else{

        PIXEL00_81
        PIXEL01_31
        PIXEL02_32
        PIXEL03_82
        PIXEL10_81
        PIXEL11_31
        PIXEL12_32
        PIXEL13_82
        PIXEL20_60
        PIXEL21_70
        PIXEL22_70
        PIXEL23_60
        PIXEL30_20
        PIXEL31_60
        PIXEL32_60
        PIXEL33_20
        

}

}

}

}else{
if(pattern<140){
if(pattern<138){
if(pattern<137){

        PIXEL00_80
        PIXEL01_61
        PIXEL02_60
        PIXEL03_20
        PIXEL10_10
        PIXEL11_30
        PIXEL12_70
        PIXEL13_60
        PIXEL20_10
        PIXEL21_30
        PIXEL22_70
        PIXEL23_60
        PIXEL30_80
        PIXEL31_61
        PIXEL32_60
        PIXEL33_20
        

}else{

        PIXEL00_82
        PIXEL01_82
        PIXEL02_60
        PIXEL03_20
        PIXEL10_32
        PIXEL11_32
        PIXEL12_70
        PIXEL13_60
        PIXEL20_10
        PIXEL21_30
        PIXEL22_70
        PIXEL23_60
        PIXEL30_80
        PIXEL31_61
        PIXEL32_60
        PIXEL33_20
        

}

}else{
if(pattern<139){

        if (Diff(yy[4], yy[2])) {
                PIXEL00_80
                PIXEL01_10
                PIXEL10_10
                PIXEL11_30
        } else {
                PIXEL00_50
                PIXEL01_50
                PIXEL10_50
                PIXEL11_0
        }
        PIXEL02_10
        PIXEL03_80
        PIXEL12_30
        PIXEL13_61
        PIXEL20_10
        PIXEL21_30
        PIXEL22_70
        PIXEL23_60
        PIXEL30_80
        PIXEL31_61
        PIXEL32_60
        PIXEL33_20
        

}else{

        if (Diff(yy[4], yy[2])) {
                PIXEL00_0
                PIXEL01_0
                PIXEL10_0
        } else {
                PIXEL00_50
                PIXEL01_50
                PIXEL10_50
        }
        PIXEL02_10
        PIXEL03_80
        PIXEL11_0
        PIXEL12_30
        PIXEL13_61
        PIXEL20_10
        PIXEL21_30
        PIXEL22_70
        PIXEL23_60
        PIXEL30_80
        PIXEL31_61
        PIXEL32_60
        PIXEL33_20
        

}

}

}else{
if(pattern<142){
if(pattern<141){

        PIXEL00_80
        PIXEL01_61
        PIXEL02_60
        PIXEL03_20
        PIXEL10_10
        PIXEL11_30
        PIXEL12_70
        PIXEL13_60
        PIXEL20_10
        PIXEL21_30
        PIXEL22_70
        PIXEL23_60
        PIXEL30_80
        PIXEL31_61
        PIXEL32_60
        PIXEL33_20
        

}else{

        PIXEL00_82
        PIXEL01_82
        PIXEL02_60
        PIXEL03_20
        PIXEL10_32
        PIXEL11_32
        PIXEL12_70
        PIXEL13_60
        PIXEL20_10
        PIXEL21_30
        PIXEL22_70
        PIXEL23_60
        PIXEL30_80
        PIXEL31_61
        PIXEL32_60
        PIXEL33_20
        

}

}else{
if(pattern<143){

        if (Diff(yy[4], yy[2])) {
                PIXEL00_80
                PIXEL01_10
                PIXEL02_32
                PIXEL03_82
                PIXEL10_10
                PIXEL11_30
        } else {
                PIXEL00_50
                PIXEL01_83
                PIXEL02_13
                PIXEL03_11
                PIXEL10_21
                PIXEL11_70
        }
        PIXEL12_32
        PIXEL13_82
        PIXEL20_10
        PIXEL21_30
        PIXEL22_70
        PIXEL23_60
        PIXEL30_80
        PIXEL31_61
        PIXEL32_60
        PIXEL33_20
        

}else{

        if (Diff(yy[4], yy[2])) {
                PIXEL00_0
                PIXEL01_0
                PIXEL02_32
                PIXEL03_82
                PIXEL10_0
                PIXEL11_0
        } else {
                PIXEL00_50
                PIXEL01_83
                PIXEL02_13
                PIXEL03_11
                PIXEL10_21
                PIXEL11_70
        }
        PIXEL12_32
        PIXEL13_82
        PIXEL20_10
        PIXEL21_30
        PIXEL22_70
        PIXEL23_60
        PIXEL30_80
        PIXEL31_61
        PIXEL32_60
        PIXEL33_20
        

}

}

}

}

}else{
if(pattern<152){
if(pattern<148){
if(pattern<146){
if(pattern<145){

        PIXEL00_20
        PIXEL01_60
        PIXEL02_61
        PIXEL03_80
        PIXEL10_60
        PIXEL11_70
        PIXEL12_30
        PIXEL13_10
        PIXEL20_60
        PIXEL21_70
        PIXEL22_32
        PIXEL23_32
        PIXEL30_20
        PIXEL31_60
        PIXEL32_82
        PIXEL33_82
        

}else{

        PIXEL00_20
        PIXEL01_60
        PIXEL02_61
        PIXEL03_80
        PIXEL10_60
        PIXEL11_70
        PIXEL12_30
        PIXEL13_10
        PIXEL20_60
        PIXEL21_70
        PIXEL22_32
        PIXEL23_32
        PIXEL30_20
        PIXEL31_60
        PIXEL32_82
        PIXEL33_82
        

}

}else{
if(pattern<147){

        PIXEL00_80
        PIXEL01_10
        if (Diff(yy[2], yy[6])) {
                PIXEL02_10
                PIXEL03_80
                PIXEL12_30
                PIXEL13_10
                PIXEL23_32
                PIXEL33_82
        } else {
                PIXEL02_21
                PIXEL03_50
                PIXEL12_70
                PIXEL13_83
                PIXEL23_13
                PIXEL33_11
        }
        PIXEL10_61
        PIXEL11_30
        PIXEL20_60
        PIXEL21_70
        PIXEL22_32
        PIXEL30_20
        PIXEL31_60
        PIXEL32_82
        

}else{

        PIXEL00_81
        PIXEL01_31
        if (Diff(yy[2], yy[6])) {
                PIXEL02_10
                PIXEL03_80
                PIXEL12_30
                PIXEL13_10
        } else {
                PIXEL02_11
                PIXEL03_20
                PIXEL12_0
                PIXEL13_12
        }
        PIXEL10_81
        PIXEL11_31
        PIXEL20_60
        PIXEL21_70
        PIXEL22_32
        PIXEL23_32
        PIXEL30_20
        PIXEL31_60
        PIXEL32_82
        PIXEL33_82
        

}

}

}else{
if(pattern<150){
if(pattern<149){

        PIXEL00_20
        PIXEL01_60
        PIXEL02_81
        PIXEL03_81
        PIXEL10_60
        PIXEL11_70
        PIXEL12_31
        PIXEL13_31
        PIXEL20_60
        PIXEL21_70
        PIXEL22_32
        PIXEL23_32
        PIXEL30_20
        PIXEL31_60
        PIXEL32_82
        PIXEL33_82
        

}else{

        PIXEL00_20
        PIXEL01_60
        PIXEL02_81
        PIXEL03_81
        PIXEL10_60
        PIXEL11_70
        PIXEL12_31
        PIXEL13_31
        PIXEL20_60
        PIXEL21_70
        PIXEL22_32
        PIXEL23_32
        PIXEL30_20
        PIXEL31_60
        PIXEL32_82
        PIXEL33_82
        

}

}else{
if(pattern<151){

        PIXEL00_80
        PIXEL01_10
        if (Diff(yy[2], yy[6])) {
                PIXEL02_0
                PIXEL03_0
                PIXEL12_0
                PIXEL13_0
                PIXEL23_32
                PIXEL33_82
        } else {
                PIXEL02_21
                PIXEL03_50
                PIXEL12_70
                PIXEL13_83
                PIXEL23_13
                PIXEL33_11
        }
        PIXEL10_61
        PIXEL11_30
        PIXEL20_60
        PIXEL21_70
        PIXEL22_32
        PIXEL30_20
        PIXEL31_60
        PIXEL32_82
        

}else{

        PIXEL00_81
        PIXEL01_31
        PIXEL02_0
        if (Diff(yy[2], yy[6])) {
                PIXEL03_0
        } else {
                PIXEL03_20
        }
        PIXEL10_81
        PIXEL11_31
        PIXEL12_0
        PIXEL13_0
        PIXEL20_60
        PIXEL21_70
        PIXEL22_32
        PIXEL23_32
        PIXEL30_20
        PIXEL31_60
        PIXEL32_82
        PIXEL33_82
        

}

}

}

}else{
if(pattern<156){
if(pattern<154){
if(pattern<153){

        PIXEL00_80
        PIXEL01_61
        PIXEL02_61
        PIXEL03_80
        PIXEL10_10
        PIXEL11_30
        PIXEL12_30
        PIXEL13_10
        PIXEL20_10
        PIXEL21_30
        PIXEL22_32
        PIXEL23_32
        PIXEL30_80
        PIXEL31_61
        PIXEL32_82
        PIXEL33_82
        

}else{

        PIXEL00_82
        PIXEL01_82
        PIXEL02_61
        PIXEL03_80
        PIXEL10_32
        PIXEL11_32
        PIXEL12_30
        PIXEL13_10
        PIXEL20_10
        PIXEL21_30
        PIXEL22_32
        PIXEL23_32
        PIXEL30_80
        PIXEL31_61
        PIXEL32_82
        PIXEL33_82
        

}

}else{
if(pattern<155){

        if (Diff(yy[4], yy[2])) {
                PIXEL00_80
                PIXEL01_10
                PIXEL10_10
                PIXEL11_30
        } else {
                PIXEL00_20
                PIXEL01_12
                PIXEL10_11
                PIXEL11_0
        }
        if (Diff(yy[2], yy[6])) {
                PIXEL02_10
                PIXEL03_80
                PIXEL12_30
                PIXEL13_10
        } else {
                PIXEL02_11
                PIXEL03_20
                PIXEL12_0
                PIXEL13_12
        }
        PIXEL20_10
        PIXEL21_30
        PIXEL22_32
        PIXEL23_32
        PIXEL30_80
        PIXEL31_61
        PIXEL32_82
        PIXEL33_82
        

}else{

        if (Diff(yy[4], yy[2])) {
                PIXEL00_0
                PIXEL01_0
                PIXEL10_0
        } else {
                PIXEL00_50
                PIXEL01_50
                PIXEL10_50
        }
        PIXEL02_10
        PIXEL03_80
        PIXEL11_0
        PIXEL12_30
        PIXEL13_10
        PIXEL20_10
        PIXEL21_30
        PIXEL22_32
        PIXEL23_32
        PIXEL30_80
        PIXEL31_61
        PIXEL32_82
        PIXEL33_82
        

}

}

}else{
if(pattern<158){
if(pattern<157){

        PIXEL00_80
        PIXEL01_61
        PIXEL02_81
        PIXEL03_81
        PIXEL10_10
        PIXEL11_30
        PIXEL12_31
        PIXEL13_31
        PIXEL20_10
        PIXEL21_30
        PIXEL22_32
        PIXEL23_32
        PIXEL30_80
        PIXEL31_61
        PIXEL32_82
        PIXEL33_82
        

}else{

        PIXEL00_82
        PIXEL01_82
        PIXEL02_81
        PIXEL03_81
        PIXEL10_32
        PIXEL11_32
        PIXEL12_31
        PIXEL13_31
        PIXEL20_10
        PIXEL21_30
        PIXEL22_32
        PIXEL23_32
        PIXEL30_80
        PIXEL31_61
        PIXEL32_82
        PIXEL33_82
        

}

}else{
if(pattern<159){

        if (Diff(yy[4], yy[2])) {
                PIXEL00_80
                PIXEL01_10
                PIXEL10_10
                PIXEL11_30
        } else {
                PIXEL00_20
                PIXEL01_12
                PIXEL10_11
                PIXEL11_0
        }
        if (Diff(yy[2], yy[6])) {
                PIXEL02_0
                PIXEL03_0
                PIXEL13_0
        } else {
                PIXEL02_50
                PIXEL03_50
                PIXEL13_50
        }
        PIXEL12_0
        PIXEL20_10
        PIXEL21_30
        PIXEL22_32
        PIXEL23_32
        PIXEL30_80
        PIXEL31_61
        PIXEL32_82
        PIXEL33_82
        

}else{

        if (Diff(yy[4], yy[2])) {
                PIXEL00_0
                PIXEL01_0
                PIXEL10_0
        } else {
                PIXEL00_50
                PIXEL01_50
                PIXEL10_50
        }
        PIXEL02_0
        if (Diff(yy[2], yy[6])) {
                PIXEL03_0
        } else {
                PIXEL03_20
        }
        PIXEL11_0
        PIXEL12_0
        PIXEL13_0
        PIXEL20_10
        PIXEL21_30
        PIXEL22_32
        PIXEL23_32
        PIXEL30_80
        PIXEL31_61
        PIXEL32_82
        PIXEL33_82
        

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

        PIXEL00_20
        PIXEL01_60
        PIXEL02_60
        PIXEL03_20
        PIXEL10_60
        PIXEL11_70
        PIXEL12_70
        PIXEL13_60
        PIXEL20_60
        PIXEL21_70
        PIXEL22_70
        PIXEL23_60
        PIXEL30_20
        PIXEL31_60
        PIXEL32_60
        PIXEL33_20
        

}else{

        PIXEL00_20
        PIXEL01_60
        PIXEL02_60
        PIXEL03_20
        PIXEL10_60
        PIXEL11_70
        PIXEL12_70
        PIXEL13_60
        PIXEL20_60
        PIXEL21_70
        PIXEL22_70
        PIXEL23_60
        PIXEL30_20
        PIXEL31_60
        PIXEL32_60
        PIXEL33_20
        

}

}else{
if(pattern<163){

        PIXEL00_80
        PIXEL01_10
        PIXEL02_10
        PIXEL03_80
        PIXEL10_61
        PIXEL11_30
        PIXEL12_30
        PIXEL13_61
        PIXEL20_60
        PIXEL21_70
        PIXEL22_70
        PIXEL23_60
        PIXEL30_20
        PIXEL31_60
        PIXEL32_60
        PIXEL33_20
        

}else{

        PIXEL00_81
        PIXEL01_31
        PIXEL02_10
        PIXEL03_80
        PIXEL10_81
        PIXEL11_31
        PIXEL12_30
        PIXEL13_61
        PIXEL20_60
        PIXEL21_70
        PIXEL22_70
        PIXEL23_60
        PIXEL30_20
        PIXEL31_60
        PIXEL32_60
        PIXEL33_20
        

}

}

}else{
if(pattern<166){
if(pattern<165){

        PIXEL00_20
        PIXEL01_60
        PIXEL02_60
        PIXEL03_20
        PIXEL10_60
        PIXEL11_70
        PIXEL12_70
        PIXEL13_60
        PIXEL20_60
        PIXEL21_70
        PIXEL22_70
        PIXEL23_60
        PIXEL30_20
        PIXEL31_60
        PIXEL32_60
        PIXEL33_20
        

}else{

        PIXEL00_20
        PIXEL01_60
        PIXEL02_60
        PIXEL03_20
        PIXEL10_60
        PIXEL11_70
        PIXEL12_70
        PIXEL13_60
        PIXEL20_60
        PIXEL21_70
        PIXEL22_70
        PIXEL23_60
        PIXEL30_20
        PIXEL31_60
        PIXEL32_60
        PIXEL33_20
        

}

}else{
if(pattern<167){

        PIXEL00_80
        PIXEL01_10
        PIXEL02_32
        PIXEL03_82
        PIXEL10_61
        PIXEL11_30
        PIXEL12_32
        PIXEL13_82
        PIXEL20_60
        PIXEL21_70
        PIXEL22_70
        PIXEL23_60
        PIXEL30_20
        PIXEL31_60
        PIXEL32_60
        PIXEL33_20
        

}else{

        PIXEL00_81
        PIXEL01_31
        PIXEL02_32
        PIXEL03_82
        PIXEL10_81
        PIXEL11_31
        PIXEL12_32
        PIXEL13_82
        PIXEL20_60
        PIXEL21_70
        PIXEL22_70
        PIXEL23_60
        PIXEL30_20
        PIXEL31_60
        PIXEL32_60
        PIXEL33_20
        

}

}

}

}else{
if(pattern<172){
if(pattern<170){
if(pattern<169){

        PIXEL00_80
        PIXEL01_61
        PIXEL02_60
        PIXEL03_20
        PIXEL10_10
        PIXEL11_30
        PIXEL12_70
        PIXEL13_60
        PIXEL20_31
        PIXEL21_31
        PIXEL22_70
        PIXEL23_60
        PIXEL30_81
        PIXEL31_81
        PIXEL32_60
        PIXEL33_20
        

}else{

        PIXEL00_82
        PIXEL01_82
        PIXEL02_60
        PIXEL03_20
        PIXEL10_32
        PIXEL11_32
        PIXEL12_70
        PIXEL13_60
        PIXEL20_31
        PIXEL21_31
        PIXEL22_70
        PIXEL23_60
        PIXEL30_81
        PIXEL31_81
        PIXEL32_60
        PIXEL33_20
        

}

}else{
if(pattern<171){

        if (Diff(yy[4], yy[2])) {
                PIXEL00_80
                PIXEL01_10
                PIXEL10_10
                PIXEL11_30
                PIXEL20_31
                PIXEL30_81
        } else {
                PIXEL00_50
                PIXEL01_21
                PIXEL10_83
                PIXEL11_70
                PIXEL20_14
                PIXEL30_12
        }
        PIXEL02_10
        PIXEL03_80
        PIXEL12_30
        PIXEL13_61
        PIXEL21_31
        PIXEL22_70
        PIXEL23_60
        PIXEL31_81
        PIXEL32_60
        PIXEL33_20
        

}else{

        if (Diff(yy[4], yy[2])) {
                PIXEL00_0
                PIXEL01_0
                PIXEL10_0
                PIXEL11_0
                PIXEL20_31
                PIXEL30_81
        } else {
                PIXEL00_50
                PIXEL01_21
                PIXEL10_83
                PIXEL11_70
                PIXEL20_14
                PIXEL30_12
        }
        PIXEL02_10
        PIXEL03_80
        PIXEL12_30
        PIXEL13_61
        PIXEL21_31
        PIXEL22_70
        PIXEL23_60
        PIXEL31_81
        PIXEL32_60
        PIXEL33_20
        

}

}

}else{
if(pattern<174){
if(pattern<173){

        PIXEL00_80
        PIXEL01_61
        PIXEL02_60
        PIXEL03_20
        PIXEL10_10
        PIXEL11_30
        PIXEL12_70
        PIXEL13_60
        PIXEL20_31
        PIXEL21_31
        PIXEL22_70
        PIXEL23_60
        PIXEL30_81
        PIXEL31_81
        PIXEL32_60
        PIXEL33_20
        

}else{

        PIXEL00_82
        PIXEL01_82
        PIXEL02_60
        PIXEL03_20
        PIXEL10_32
        PIXEL11_32
        PIXEL12_70
        PIXEL13_60
        PIXEL20_31
        PIXEL21_31
        PIXEL22_70
        PIXEL23_60
        PIXEL30_81
        PIXEL31_81
        PIXEL32_60
        PIXEL33_20
        

}

}else{
if(pattern<175){

        if (Diff(yy[4], yy[2])) {
                PIXEL00_80
                PIXEL01_10
                PIXEL10_10
                PIXEL11_30
        } else {
                PIXEL00_20
                PIXEL01_12
                PIXEL10_11
                PIXEL11_0
        }
        PIXEL02_32
        PIXEL03_82
        PIXEL12_32
        PIXEL13_82
        PIXEL20_31
        PIXEL21_31
        PIXEL22_70
        PIXEL23_60
        PIXEL30_81
        PIXEL31_81
        PIXEL32_60
        PIXEL33_20
        

}else{

        if (Diff(yy[4], yy[2])) {
                PIXEL00_0
        } else {
                PIXEL00_20
        }
        PIXEL01_0
        PIXEL02_32
        PIXEL03_82
        PIXEL10_0
        PIXEL11_0
        PIXEL12_32
        PIXEL13_82
        PIXEL20_31
        PIXEL21_31
        PIXEL22_70
        PIXEL23_60
        PIXEL30_81
        PIXEL31_81
        PIXEL32_60
        PIXEL33_20
        

}

}

}

}

}else{
if(pattern<184){
if(pattern<180){
if(pattern<178){
if(pattern<177){

        PIXEL00_20
        PIXEL01_60
        PIXEL02_61
        PIXEL03_80
        PIXEL10_60
        PIXEL11_70
        PIXEL12_30
        PIXEL13_10
        PIXEL20_60
        PIXEL21_70
        PIXEL22_32
        PIXEL23_32
        PIXEL30_20
        PIXEL31_60
        PIXEL32_82
        PIXEL33_82
        

}else{

        PIXEL00_20
        PIXEL01_60
        PIXEL02_61
        PIXEL03_80
        PIXEL10_60
        PIXEL11_70
        PIXEL12_30
        PIXEL13_10
        PIXEL20_60
        PIXEL21_70
        PIXEL22_32
        PIXEL23_32
        PIXEL30_20
        PIXEL31_60
        PIXEL32_82
        PIXEL33_82
        

}

}else{
if(pattern<179){

        PIXEL00_80
        PIXEL01_10
        if (Diff(yy[2], yy[6])) {
                PIXEL02_10
                PIXEL03_80
                PIXEL12_30
                PIXEL13_10
                PIXEL23_32
                PIXEL33_82
        } else {
                PIXEL02_21
                PIXEL03_50
                PIXEL12_70
                PIXEL13_83
                PIXEL23_13
                PIXEL33_11
        }
        PIXEL10_61
        PIXEL11_30
        PIXEL20_60
        PIXEL21_70
        PIXEL22_32
        PIXEL30_20
        PIXEL31_60
        PIXEL32_82
        

}else{

        PIXEL00_81
        PIXEL01_31
        if (Diff(yy[2], yy[6])) {
                PIXEL02_10
                PIXEL03_80
                PIXEL12_30
                PIXEL13_10
        } else {
                PIXEL02_11
                PIXEL03_20
                PIXEL12_0
                PIXEL13_12
        }
        PIXEL10_81
        PIXEL11_31
        PIXEL20_60
        PIXEL21_70
        PIXEL22_32
        PIXEL23_32
        PIXEL30_20
        PIXEL31_60
        PIXEL32_82
        PIXEL33_82
        

}

}

}else{
if(pattern<182){
if(pattern<181){

        PIXEL00_20
        PIXEL01_60
        PIXEL02_81
        PIXEL03_81
        PIXEL10_60
        PIXEL11_70
        PIXEL12_31
        PIXEL13_31
        PIXEL20_60
        PIXEL21_70
        PIXEL22_32
        PIXEL23_32
        PIXEL30_20
        PIXEL31_60
        PIXEL32_82
        PIXEL33_82
        

}else{

        PIXEL00_20
        PIXEL01_60
        PIXEL02_81
        PIXEL03_81
        PIXEL10_60
        PIXEL11_70
        PIXEL12_31
        PIXEL13_31
        PIXEL20_60
        PIXEL21_70
        PIXEL22_32
        PIXEL23_32
        PIXEL30_20
        PIXEL31_60
        PIXEL32_82
        PIXEL33_82
        

}

}else{
if(pattern<183){

        PIXEL00_80
        PIXEL01_10
        if (Diff(yy[2], yy[6])) {
                PIXEL02_0
                PIXEL03_0
                PIXEL12_0
                PIXEL13_0
                PIXEL23_32
                PIXEL33_82
        } else {
                PIXEL02_21
                PIXEL03_50
                PIXEL12_70
                PIXEL13_83
                PIXEL23_13
                PIXEL33_11
        }
        PIXEL10_61
        PIXEL11_30
        PIXEL20_60
        PIXEL21_70
        PIXEL22_32
        PIXEL30_20
        PIXEL31_60
        PIXEL32_82
        

}else{

        PIXEL00_81
        PIXEL01_31
        PIXEL02_0
        if (Diff(yy[2], yy[6])) {
                PIXEL03_0
        } else {
                PIXEL03_20
        }
        PIXEL10_81
        PIXEL11_31
        PIXEL12_0
        PIXEL13_0
        PIXEL20_60
        PIXEL21_70
        PIXEL22_32
        PIXEL23_32
        PIXEL30_20
        PIXEL31_60
        PIXEL32_82
        PIXEL33_82
        

}

}

}

}else{
if(pattern<188){
if(pattern<186){
if(pattern<185){

        PIXEL00_80
        PIXEL01_61
        PIXEL02_61
        PIXEL03_80
        PIXEL10_10
        PIXEL11_30
        PIXEL12_30
        PIXEL13_10
        PIXEL20_31
        PIXEL21_31
        PIXEL22_32
        PIXEL23_32
        PIXEL30_81
        PIXEL31_81
        PIXEL32_82
        PIXEL33_82
        

}else{

        PIXEL00_82
        PIXEL01_82
        PIXEL02_61
        PIXEL03_80
        PIXEL10_32
        PIXEL11_32
        PIXEL12_30
        PIXEL13_10
        PIXEL20_31
        PIXEL21_31
        PIXEL22_32
        PIXEL23_32
        PIXEL30_81
        PIXEL31_81
        PIXEL32_82
        PIXEL33_82
        

}

}else{
if(pattern<187){

        if (Diff(yy[4], yy[2])) {
                PIXEL00_80
                PIXEL01_10
                PIXEL10_10
                PIXEL11_30
        } else {
                PIXEL00_20
                PIXEL01_12
                PIXEL10_11
                PIXEL11_0
        }
        if (Diff(yy[2], yy[6])) {
                PIXEL02_10
                PIXEL03_80
                PIXEL12_30
                PIXEL13_10
        } else {
                PIXEL02_11
                PIXEL03_20
                PIXEL12_0
                PIXEL13_12
        }
        PIXEL20_31
        PIXEL21_31
        PIXEL22_32
        PIXEL23_32
        PIXEL30_81
        PIXEL31_81
        PIXEL32_82
        PIXEL33_82
        

}else{

        if (Diff(yy[4], yy[2])) {
                PIXEL00_0
                PIXEL01_0
                PIXEL10_0
                PIXEL11_0
                PIXEL20_31
                PIXEL30_81
        } else {
                PIXEL00_50
                PIXEL01_21
                PIXEL10_83
                PIXEL11_70
                PIXEL20_14
                PIXEL30_12
        }
        PIXEL02_10
        PIXEL03_80
        PIXEL12_30
        PIXEL13_10
        PIXEL21_31
        PIXEL22_32
        PIXEL23_32
        PIXEL31_81
        PIXEL32_82
        PIXEL33_82
        

}

}

}else{
if(pattern<190){
if(pattern<189){

        PIXEL00_80
        PIXEL01_61
        PIXEL02_81
        PIXEL03_81
        PIXEL10_10
        PIXEL11_30
        PIXEL12_31
        PIXEL13_31
        PIXEL20_31
        PIXEL21_31
        PIXEL22_32
        PIXEL23_32
        PIXEL30_81
        PIXEL31_81
        PIXEL32_82
        PIXEL33_82
        

}else{

        PIXEL00_82
        PIXEL01_82
        PIXEL02_81
        PIXEL03_81
        PIXEL10_32
        PIXEL11_32
        PIXEL12_31
        PIXEL13_31
        PIXEL20_31
        PIXEL21_31
        PIXEL22_32
        PIXEL23_32
        PIXEL30_81
        PIXEL31_81
        PIXEL32_82
        PIXEL33_82
        

}

}else{
if(pattern<191){

        PIXEL00_80
        PIXEL01_10
        if (Diff(yy[2], yy[6])) {
                PIXEL02_0
                PIXEL03_0
                PIXEL12_0
                PIXEL13_0
                PIXEL23_32
                PIXEL33_82
        } else {
                PIXEL02_21
                PIXEL03_50
                PIXEL12_70
                PIXEL13_83
                PIXEL23_13
                PIXEL33_11
        }
        PIXEL10_10
        PIXEL11_30
        PIXEL20_31
        PIXEL21_31
        PIXEL22_32
        PIXEL30_81
        PIXEL31_81
        PIXEL32_82
        

}else{

        if (Diff(yy[4], yy[2])) {
                PIXEL00_0
        } else {
                PIXEL00_20
        }
        PIXEL01_0
        PIXEL02_0
        if (Diff(yy[2], yy[6])) {
                PIXEL03_0
        } else {
                PIXEL03_20
        }
        PIXEL10_0
        PIXEL11_0
        PIXEL12_0
        PIXEL13_0
        PIXEL20_31
        PIXEL21_31
        PIXEL22_32
        PIXEL23_32
        PIXEL30_81
        PIXEL31_81
        PIXEL32_82
        PIXEL33_82
        

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

        PIXEL00_20
        PIXEL01_60
        PIXEL02_60
        PIXEL03_20
        PIXEL10_60
        PIXEL11_70
        PIXEL12_70
        PIXEL13_60
        PIXEL20_61
        PIXEL21_30
        PIXEL22_31
        PIXEL23_81
        PIXEL30_80
        PIXEL31_10
        PIXEL32_31
        PIXEL33_81
        

}else{

        PIXEL00_20
        PIXEL01_60
        PIXEL02_60
        PIXEL03_20
        PIXEL10_60
        PIXEL11_70
        PIXEL12_70
        PIXEL13_60
        PIXEL20_61
        PIXEL21_30
        PIXEL22_31
        PIXEL23_81
        PIXEL30_80
        PIXEL31_10
        PIXEL32_31
        PIXEL33_81
        

}

}else{
if(pattern<195){

        PIXEL00_80
        PIXEL01_10
        PIXEL02_10
        PIXEL03_80
        PIXEL10_61
        PIXEL11_30
        PIXEL12_30
        PIXEL13_61
        PIXEL20_61
        PIXEL21_30
        PIXEL22_31
        PIXEL23_81
        PIXEL30_80
        PIXEL31_10
        PIXEL32_31
        PIXEL33_81
        

}else{

        PIXEL00_81
        PIXEL01_31
        PIXEL02_10
        PIXEL03_80
        PIXEL10_81
        PIXEL11_31
        PIXEL12_30
        PIXEL13_61
        PIXEL20_61
        PIXEL21_30
        PIXEL22_31
        PIXEL23_81
        PIXEL30_80
        PIXEL31_10
        PIXEL32_31
        PIXEL33_81
        

}

}

}else{
if(pattern<198){
if(pattern<197){

        PIXEL00_20
        PIXEL01_60
        PIXEL02_60
        PIXEL03_20
        PIXEL10_60
        PIXEL11_70
        PIXEL12_70
        PIXEL13_60
        PIXEL20_61
        PIXEL21_30
        PIXEL22_31
        PIXEL23_81
        PIXEL30_80
        PIXEL31_10
        PIXEL32_31
        PIXEL33_81
        

}else{

        PIXEL00_20
        PIXEL01_60
        PIXEL02_60
        PIXEL03_20
        PIXEL10_60
        PIXEL11_70
        PIXEL12_70
        PIXEL13_60
        PIXEL20_61
        PIXEL21_30
        PIXEL22_31
        PIXEL23_81
        PIXEL30_80
        PIXEL31_10
        PIXEL32_31
        PIXEL33_81
        

}

}else{
if(pattern<199){

        PIXEL00_80
        PIXEL01_10
        PIXEL02_32
        PIXEL03_82
        PIXEL10_61
        PIXEL11_30
        PIXEL12_32
        PIXEL13_82
        PIXEL20_61
        PIXEL21_30
        PIXEL22_31
        PIXEL23_81
        PIXEL30_80
        PIXEL31_10
        PIXEL32_31
        PIXEL33_81
        

}else{

        PIXEL00_81
        PIXEL01_31
        PIXEL02_32
        PIXEL03_82
        PIXEL10_81
        PIXEL11_31
        PIXEL12_32
        PIXEL13_82
        PIXEL20_61
        PIXEL21_30
        PIXEL22_31
        PIXEL23_81
        PIXEL30_80
        PIXEL31_10
        PIXEL32_31
        PIXEL33_81
        

}

}

}

}else{
if(pattern<204){
if(pattern<202){
if(pattern<201){

        PIXEL00_80
        PIXEL01_61
        PIXEL02_60
        PIXEL03_20
        PIXEL10_10
        PIXEL11_30
        PIXEL12_70
        PIXEL13_60
        if (Diff(yy[8], yy[4])) {
                PIXEL20_10
                PIXEL21_30
                PIXEL30_80
                PIXEL31_10
                PIXEL32_31
                PIXEL33_81
        } else {
                PIXEL20_21
                PIXEL21_70
                PIXEL30_50
                PIXEL31_83
                PIXEL32_14
                PIXEL33_12
        }
        PIXEL22_31
        PIXEL23_81
        

}else{

        PIXEL00_82
        PIXEL01_82
        PIXEL02_60
        PIXEL03_20
        PIXEL10_32
        PIXEL11_32
        PIXEL12_70
        PIXEL13_60
        if (Diff(yy[8], yy[4])) {
                PIXEL20_10
                PIXEL21_30
                PIXEL30_80
                PIXEL31_10
        } else {
                PIXEL20_12
                PIXEL21_0
                PIXEL30_20
                PIXEL31_11
        }
        PIXEL22_31
        PIXEL23_81
        PIXEL32_31
        PIXEL33_81
        

}

}else{
if(pattern<203){

        if (Diff(yy[4], yy[2])) {
                PIXEL00_80
                PIXEL01_10
                PIXEL10_10
                PIXEL11_30
        } else {
                PIXEL00_20
                PIXEL01_12
                PIXEL10_11
                PIXEL11_0
        }
        PIXEL02_10
        PIXEL03_80
        PIXEL12_30
        PIXEL13_61
        if (Diff(yy[8], yy[4])) {
                PIXEL20_10
                PIXEL21_30
                PIXEL30_80
                PIXEL31_10
        } else {
                PIXEL20_12
                PIXEL21_0
                PIXEL30_20
                PIXEL31_11
        }
        PIXEL22_31
        PIXEL23_81
        PIXEL32_31
        PIXEL33_81
        

}else{

        if (Diff(yy[4], yy[2])) {
                PIXEL00_0
                PIXEL01_0
                PIXEL10_0
        } else {
                PIXEL00_50
                PIXEL01_50
                PIXEL10_50
        }
        PIXEL02_10
        PIXEL03_80
        PIXEL11_0
        PIXEL12_30
        PIXEL13_61
        PIXEL20_10
        PIXEL21_30
        PIXEL22_31
        PIXEL23_81
        PIXEL30_80
        PIXEL31_10
        PIXEL32_31
        PIXEL33_81
        

}

}

}else{
if(pattern<206){
if(pattern<205){

        PIXEL00_80
        PIXEL01_61
        PIXEL02_60
        PIXEL03_20
        PIXEL10_10
        PIXEL11_30
        PIXEL12_70
        PIXEL13_60
        if (Diff(yy[8], yy[4])) {
                PIXEL20_10
                PIXEL21_30
                PIXEL30_80
                PIXEL31_10
                PIXEL32_31
                PIXEL33_81
        } else {
                PIXEL20_21
                PIXEL21_70
                PIXEL30_50
                PIXEL31_83
                PIXEL32_14
                PIXEL33_12
        }
        PIXEL22_31
        PIXEL23_81
        

}else{

        PIXEL00_82
        PIXEL01_82
        PIXEL02_60
        PIXEL03_20
        PIXEL10_32
        PIXEL11_32
        PIXEL12_70
        PIXEL13_60
        if (Diff(yy[8], yy[4])) {
                PIXEL20_10
                PIXEL21_30
                PIXEL30_80
                PIXEL31_10
        } else {
                PIXEL20_12
                PIXEL21_0
                PIXEL30_20
                PIXEL31_11
        }
        PIXEL22_31
        PIXEL23_81
        PIXEL32_31
        PIXEL33_81
        

}

}else{
if(pattern<207){

        if (Diff(yy[4], yy[2])) {
                PIXEL00_80
                PIXEL01_10
                PIXEL10_10
                PIXEL11_30
        } else {
                PIXEL00_20
                PIXEL01_12
                PIXEL10_11
                PIXEL11_0
        }
        PIXEL02_32
        PIXEL03_82
        PIXEL12_32
        PIXEL13_82
        if (Diff(yy[8], yy[4])) {
                PIXEL20_10
                PIXEL21_30
                PIXEL30_80
                PIXEL31_10
        } else {
                PIXEL20_12
                PIXEL21_0
                PIXEL30_20
                PIXEL31_11
        }
        PIXEL22_31
        PIXEL23_81
        PIXEL32_31
        PIXEL33_81
        

}else{

        if (Diff(yy[4], yy[2])) {
                PIXEL00_0
                PIXEL01_0
                PIXEL02_32
                PIXEL03_82
                PIXEL10_0
                PIXEL11_0
        } else {
                PIXEL00_50
                PIXEL01_83
                PIXEL02_13
                PIXEL03_11
                PIXEL10_21
                PIXEL11_70
        }
        PIXEL12_32
        PIXEL13_82
        PIXEL20_10
        PIXEL21_30
        PIXEL22_31
        PIXEL23_81
        PIXEL30_80
        PIXEL31_10
        PIXEL32_31
        PIXEL33_81
        

}

}

}

}

}else{
if(pattern<216){
if(pattern<212){
if(pattern<210){
if(pattern<209){

        PIXEL00_20
        PIXEL01_60
        PIXEL02_61
        PIXEL03_80
        PIXEL10_60
        PIXEL11_70
        PIXEL12_30
        PIXEL13_10
        PIXEL20_61
        PIXEL21_30
        PIXEL22_0
        if (Diff(yy[6], yy[8])) {
                PIXEL23_0
                PIXEL32_0
                PIXEL33_0
        } else {
                PIXEL23_50
                PIXEL32_50
                PIXEL33_50
        }
        PIXEL30_80
        PIXEL31_10
        

}else{

        PIXEL00_20
        PIXEL01_60
        PIXEL02_61
        PIXEL03_80
        PIXEL10_60
        PIXEL11_70
        PIXEL12_30
        PIXEL13_10
        PIXEL20_61
        PIXEL21_30
        PIXEL22_0
        if (Diff(yy[6], yy[8])) {
                PIXEL23_0
                PIXEL32_0
                PIXEL33_0
        } else {
                PIXEL23_50
                PIXEL32_50
                PIXEL33_50
        }
        PIXEL30_80
        PIXEL31_10
        

}

}else{
if(pattern<211){

        PIXEL00_80
        PIXEL01_10
        PIXEL02_10
        PIXEL03_80
        PIXEL10_61
        PIXEL11_30
        PIXEL12_30
        PIXEL13_10
        PIXEL20_61
        PIXEL21_30
        PIXEL22_0
        if (Diff(yy[6], yy[8])) {
                PIXEL23_0
                PIXEL32_0
                PIXEL33_0
        } else {
                PIXEL23_50
                PIXEL32_50
                PIXEL33_50
        }
        PIXEL30_80
        PIXEL31_10
        

}else{

        PIXEL00_81
        PIXEL01_31
        PIXEL02_10
        PIXEL03_80
        PIXEL10_81
        PIXEL11_31
        PIXEL12_30
        PIXEL13_10
        PIXEL20_61
        PIXEL21_30
        PIXEL22_0
        if (Diff(yy[6], yy[8])) {
                PIXEL23_0
                PIXEL32_0
                PIXEL33_0
        } else {
                PIXEL23_50
                PIXEL32_50
                PIXEL33_50
        }
        PIXEL30_80
        PIXEL31_10
        

}

}

}else{
if(pattern<214){
if(pattern<213){

        PIXEL00_20
        PIXEL01_60
        PIXEL02_81
        if (Diff(yy[6], yy[8])) {
                PIXEL03_81
                PIXEL13_31
                PIXEL22_0
                PIXEL23_0
                PIXEL32_0
                PIXEL33_0
        } else {
                PIXEL03_12
                PIXEL13_14
                PIXEL22_70
                PIXEL23_83
                PIXEL32_21
                PIXEL33_50
        }
        PIXEL10_60
        PIXEL11_70
        PIXEL12_31
        PIXEL20_61
        PIXEL21_30
        PIXEL30_80
        PIXEL31_10
        

}else{

        PIXEL00_20
        PIXEL01_60
        PIXEL02_81
        if (Diff(yy[6], yy[8])) {
                PIXEL03_81
                PIXEL13_31
                PIXEL22_0
                PIXEL23_0
                PIXEL32_0
                PIXEL33_0
        } else {
                PIXEL03_12
                PIXEL13_14
                PIXEL22_70
                PIXEL23_83
                PIXEL32_21
                PIXEL33_50
        }
        PIXEL10_60
        PIXEL11_70
        PIXEL12_31
        PIXEL20_61
        PIXEL21_30
        PIXEL30_80
        PIXEL31_10
        

}

}else{
if(pattern<215){

        PIXEL00_80
        PIXEL01_10
        if (Diff(yy[2], yy[6])) {
                PIXEL02_0
                PIXEL03_0
                PIXEL13_0
        } else {
                PIXEL02_50
                PIXEL03_50
                PIXEL13_50
        }
        PIXEL10_61
        PIXEL11_30
        PIXEL12_0
        PIXEL20_61
        PIXEL21_30
        PIXEL22_0
        if (Diff(yy[6], yy[8])) {
                PIXEL23_0
                PIXEL32_0
                PIXEL33_0
        } else {
                PIXEL23_50
                PIXEL32_50
                PIXEL33_50
        }
        PIXEL30_80
        PIXEL31_10
        

}else{

        PIXEL00_81
        PIXEL01_31
        PIXEL02_0
        if (Diff(yy[2], yy[6])) {
                PIXEL03_0
        } else {
                PIXEL03_20
        }
        PIXEL10_81
        PIXEL11_31
        PIXEL12_0
        PIXEL13_0
        PIXEL20_61
        PIXEL21_30
        PIXEL22_0
        if (Diff(yy[6], yy[8])) {
                PIXEL23_0
                PIXEL32_0
                PIXEL33_0
        } else {
                PIXEL23_50
                PIXEL32_50
                PIXEL33_50
        }
        PIXEL30_80
        PIXEL31_10
        

}

}

}

}else{
if(pattern<220){
if(pattern<218){
if(pattern<217){

        PIXEL00_80
        PIXEL01_61
        PIXEL02_61
        PIXEL03_80
        PIXEL10_10
        PIXEL11_30
        PIXEL12_30
        PIXEL13_10
        PIXEL20_10
        PIXEL21_30
        PIXEL22_0
        if (Diff(yy[6], yy[8])) {
                PIXEL23_0
                PIXEL32_0
                PIXEL33_0
        } else {
                PIXEL23_50
                PIXEL32_50
                PIXEL33_50
        }
        PIXEL30_80
        PIXEL31_10
        

}else{

        PIXEL00_82
        PIXEL01_82
        PIXEL02_61
        PIXEL03_80
        PIXEL10_32
        PIXEL11_32
        PIXEL12_30
        PIXEL13_10
        PIXEL20_10
        PIXEL21_30
        PIXEL22_0
        if (Diff(yy[6], yy[8])) {
                PIXEL23_0
                PIXEL32_0
                PIXEL33_0
        } else {
                PIXEL23_50
                PIXEL32_50
                PIXEL33_50
        }
        PIXEL30_80
        PIXEL31_10
        

}

}else{
if(pattern<219){

        if (Diff(yy[4], yy[2])) {
                PIXEL00_80
                PIXEL01_10
                PIXEL10_10
                PIXEL11_30
        } else {
                PIXEL00_20
                PIXEL01_12
                PIXEL10_11
                PIXEL11_0
        }
        if (Diff(yy[2], yy[6])) {
                PIXEL02_10
                PIXEL03_80
                PIXEL12_30
                PIXEL13_10
        } else {
                PIXEL02_11
                PIXEL03_20
                PIXEL12_0
                PIXEL13_12
        }
        if (Diff(yy[8], yy[4])) {
                PIXEL20_10
                PIXEL21_30
                PIXEL30_80
                PIXEL31_10
        } else {
                PIXEL20_12
                PIXEL21_0
                PIXEL30_20
                PIXEL31_11
        }
        PIXEL22_0
        if (Diff(yy[6], yy[8])) {
                PIXEL23_0
                PIXEL32_0
                PIXEL33_0
        } else {
                PIXEL23_50
                PIXEL32_50
                PIXEL33_50
        }
        

}else{

        if (Diff(yy[4], yy[2])) {
                PIXEL00_0
                PIXEL01_0
                PIXEL10_0
        } else {
                PIXEL00_50
                PIXEL01_50
                PIXEL10_50
        }
        PIXEL02_10
        PIXEL03_80
        PIXEL11_0
        PIXEL12_30
        PIXEL13_10
        PIXEL20_10
        PIXEL21_30
        PIXEL22_0
        if (Diff(yy[6], yy[8])) {
                PIXEL23_0
                PIXEL32_0
                PIXEL33_0
        } else {
                PIXEL23_50
                PIXEL32_50
                PIXEL33_50
        }
        PIXEL30_80
        PIXEL31_10
        

}

}

}else{
if(pattern<222){
if(pattern<221){

        PIXEL00_80
        PIXEL01_61
        PIXEL02_81
        PIXEL03_81
        PIXEL10_10
        PIXEL11_30
        PIXEL12_31
        PIXEL13_31
        if (Diff(yy[8], yy[4])) {
                PIXEL20_10
                PIXEL21_30
                PIXEL30_80
                PIXEL31_10
        } else {
                PIXEL20_12
                PIXEL21_0
                PIXEL30_20
                PIXEL31_11
        }
        PIXEL22_0
        if (Diff(yy[6], yy[8])) {
                PIXEL23_0
                PIXEL32_0
                PIXEL33_0
        } else {
                PIXEL23_50
                PIXEL32_50
                PIXEL33_50
        }
        

}else{

        PIXEL00_82
        PIXEL01_82
        PIXEL02_81
        if (Diff(yy[6], yy[8])) {
                PIXEL03_81
                PIXEL13_31
                PIXEL22_0
                PIXEL23_0
                PIXEL32_0
                PIXEL33_0
        } else {
                PIXEL03_12
                PIXEL13_14
                PIXEL22_70
                PIXEL23_83
                PIXEL32_21
                PIXEL33_50
        }
        PIXEL10_32
        PIXEL11_32
        PIXEL12_31
        PIXEL20_10
        PIXEL21_30
        PIXEL30_80
        PIXEL31_10
        

}

}else{
if(pattern<223){

        PIXEL00_80
        PIXEL01_10
        if (Diff(yy[2], yy[6])) {
                PIXEL02_0
                PIXEL03_0
                PIXEL13_0
        } else {
                PIXEL02_50
                PIXEL03_50
                PIXEL13_50
        }
        PIXEL10_10
        PIXEL11_30
        PIXEL12_0
        PIXEL20_10
        PIXEL21_30
        PIXEL22_0
        if (Diff(yy[6], yy[8])) {
                PIXEL23_0
                PIXEL32_0
                PIXEL33_0
        } else {
                PIXEL23_50
                PIXEL32_50
                PIXEL33_50
        }
        PIXEL30_80
        PIXEL31_10
        

}else{

        if (Diff(yy[4], yy[2])) {
                PIXEL00_0
                PIXEL01_0
                PIXEL10_0
        } else {
                PIXEL00_50
                PIXEL01_50
                PIXEL10_50
        }
        PIXEL02_0
        if (Diff(yy[2], yy[6])) {
                PIXEL03_0
        } else {
                PIXEL03_20
        }
        PIXEL11_0
        PIXEL12_0
        PIXEL13_0
        PIXEL20_10
        PIXEL21_30
        PIXEL22_0
        if (Diff(yy[6], yy[8])) {
                PIXEL23_0
                PIXEL32_0
                PIXEL33_0
        } else {
                PIXEL23_50
                PIXEL32_50
                PIXEL33_50
        }
        PIXEL30_80
        PIXEL31_10
        

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

        PIXEL00_20
        PIXEL01_60
        PIXEL02_60
        PIXEL03_20
        PIXEL10_60
        PIXEL11_70
        PIXEL12_70
        PIXEL13_60
        PIXEL20_82
        PIXEL21_32
        PIXEL22_31
        PIXEL23_81
        PIXEL30_82
        PIXEL31_32
        PIXEL32_31
        PIXEL33_81
        

}else{

        PIXEL00_20
        PIXEL01_60
        PIXEL02_60
        PIXEL03_20
        PIXEL10_60
        PIXEL11_70
        PIXEL12_70
        PIXEL13_60
        PIXEL20_82
        PIXEL21_32
        PIXEL22_31
        PIXEL23_81
        PIXEL30_82
        PIXEL31_32
        PIXEL32_31
        PIXEL33_81
        

}

}else{
if(pattern<227){

        PIXEL00_80
        PIXEL01_10
        PIXEL02_10
        PIXEL03_80
        PIXEL10_61
        PIXEL11_30
        PIXEL12_30
        PIXEL13_61
        PIXEL20_82
        PIXEL21_32
        PIXEL22_31
        PIXEL23_81
        PIXEL30_82
        PIXEL31_32
        PIXEL32_31
        PIXEL33_81
        

}else{

        PIXEL00_81
        PIXEL01_31
        PIXEL02_10
        PIXEL03_80
        PIXEL10_81
        PIXEL11_31
        PIXEL12_30
        PIXEL13_61
        PIXEL20_82
        PIXEL21_32
        PIXEL22_31
        PIXEL23_81
        PIXEL30_82
        PIXEL31_32
        PIXEL32_31
        PIXEL33_81
        

}

}

}else{
if(pattern<230){
if(pattern<229){

        PIXEL00_20
        PIXEL01_60
        PIXEL02_60
        PIXEL03_20
        PIXEL10_60
        PIXEL11_70
        PIXEL12_70
        PIXEL13_60
        PIXEL20_82
        PIXEL21_32
        PIXEL22_31
        PIXEL23_81
        PIXEL30_82
        PIXEL31_32
        PIXEL32_31
        PIXEL33_81
        

}else{

        PIXEL00_20
        PIXEL01_60
        PIXEL02_60
        PIXEL03_20
        PIXEL10_60
        PIXEL11_70
        PIXEL12_70
        PIXEL13_60
        PIXEL20_82
        PIXEL21_32
        PIXEL22_31
        PIXEL23_81
        PIXEL30_82
        PIXEL31_32
        PIXEL32_31
        PIXEL33_81
        

}

}else{
if(pattern<231){

        PIXEL00_80
        PIXEL01_10
        PIXEL02_32
        PIXEL03_82
        PIXEL10_61
        PIXEL11_30
        PIXEL12_32
        PIXEL13_82
        PIXEL20_82
        PIXEL21_32
        PIXEL22_31
        PIXEL23_81
        PIXEL30_82
        PIXEL31_32
        PIXEL32_31
        PIXEL33_81
        

}else{

        PIXEL00_81
        PIXEL01_31
        PIXEL02_32
        PIXEL03_82
        PIXEL10_81
        PIXEL11_31
        PIXEL12_32
        PIXEL13_82
        PIXEL20_82
        PIXEL21_32
        PIXEL22_31
        PIXEL23_81
        PIXEL30_82
        PIXEL31_32
        PIXEL32_31
        PIXEL33_81
        

}

}

}

}else{
if(pattern<236){
if(pattern<234){
if(pattern<233){

        PIXEL00_80
        PIXEL01_61
        PIXEL02_60
        PIXEL03_20
        PIXEL10_10
        PIXEL11_30
        PIXEL12_70
        PIXEL13_60
        if (Diff(yy[8], yy[4])) {
                PIXEL20_0
                PIXEL21_0
                PIXEL30_0
                PIXEL31_0
                PIXEL32_31
                PIXEL33_81
        } else {
                PIXEL20_21
                PIXEL21_70
                PIXEL30_50
                PIXEL31_83
                PIXEL32_14
                PIXEL33_12
        }
        PIXEL22_31
        PIXEL23_81
        

}else{

        PIXEL00_82
        PIXEL01_82
        PIXEL02_60
        PIXEL03_20
        PIXEL10_32
        PIXEL11_32
        PIXEL12_70
        PIXEL13_60
        PIXEL20_0
        PIXEL21_0
        PIXEL22_31
        PIXEL23_81
        if (Diff(yy[8], yy[4])) {
                PIXEL30_0
        } else {
                PIXEL30_20
        }
        PIXEL31_0
        PIXEL32_31
        PIXEL33_81
        

}

}else{
if(pattern<235){

        if (Diff(yy[4], yy[2])) {
                PIXEL00_80
                PIXEL01_10
                PIXEL10_10
                PIXEL11_30
        } else {
                PIXEL00_20
                PIXEL01_12
                PIXEL10_11
                PIXEL11_0
        }
        PIXEL02_10
        PIXEL03_80
        PIXEL12_30
        PIXEL13_61
        if (Diff(yy[8], yy[4])) {
                PIXEL20_0
                PIXEL30_0
                PIXEL31_0
        } else {
                PIXEL20_50
                PIXEL30_50
                PIXEL31_50
        }
        PIXEL21_0
        PIXEL22_31
        PIXEL23_81
        PIXEL32_31
        PIXEL33_81
        

}else{

        if (Diff(yy[4], yy[2])) {
                PIXEL00_0
                PIXEL01_0
                PIXEL10_0
        } else {
                PIXEL00_50
                PIXEL01_50
                PIXEL10_50
        }
        PIXEL02_10
        PIXEL03_80
        PIXEL11_0
        PIXEL12_30
        PIXEL13_61
        PIXEL20_0
        PIXEL21_0
        PIXEL22_31
        PIXEL23_81
        if (Diff(yy[8], yy[4])) {
                PIXEL30_0
        } else {
                PIXEL30_20
        }
        PIXEL31_0
        PIXEL32_31
        PIXEL33_81
        

}

}

}else{
if(pattern<238){
if(pattern<237){

        PIXEL00_80
        PIXEL01_61
        PIXEL02_60
        PIXEL03_20
        PIXEL10_10
        PIXEL11_30
        PIXEL12_70
        PIXEL13_60
        if (Diff(yy[8], yy[4])) {
                PIXEL20_0
                PIXEL21_0
                PIXEL30_0
                PIXEL31_0
                PIXEL32_31
                PIXEL33_81
        } else {
                PIXEL20_21
                PIXEL21_70
                PIXEL30_50
                PIXEL31_83
                PIXEL32_14
                PIXEL33_12
        }
        PIXEL22_31
        PIXEL23_81
        

}else{

        PIXEL00_82
        PIXEL01_82
        PIXEL02_60
        PIXEL03_20
        PIXEL10_32
        PIXEL11_32
        PIXEL12_70
        PIXEL13_60
        PIXEL20_0
        PIXEL21_0
        PIXEL22_31
        PIXEL23_81
        if (Diff(yy[8], yy[4])) {
                PIXEL30_0
        } else {
                PIXEL30_20
        }
        PIXEL31_0
        PIXEL32_31
        PIXEL33_81
        

}

}else{
if(pattern<239){

        PIXEL00_80
        PIXEL01_10
        PIXEL02_32
        PIXEL03_82
        PIXEL10_10
        PIXEL11_30
        PIXEL12_32
        PIXEL13_82
        if (Diff(yy[8], yy[4])) {
                PIXEL20_0
                PIXEL21_0
                PIXEL30_0
                PIXEL31_0
                PIXEL32_31
                PIXEL33_81
        } else {
                PIXEL20_21
                PIXEL21_70
                PIXEL30_50
                PIXEL31_83
                PIXEL32_14
                PIXEL33_12
        }
        PIXEL22_31
        PIXEL23_81
        

}else{

        if (Diff(yy[4], yy[2])) {
                PIXEL00_0
        } else {
                PIXEL00_20
        }
        PIXEL01_0
        PIXEL02_32
        PIXEL03_82
        PIXEL10_0
        PIXEL11_0
        PIXEL12_32
        PIXEL13_82
        PIXEL20_0
        PIXEL21_0
        PIXEL22_31
        PIXEL23_81
        if (Diff(yy[8], yy[4])) {
                PIXEL30_0
        } else {
                PIXEL30_20
        }
        PIXEL31_0
        PIXEL32_31
        PIXEL33_81
        

}

}

}

}

}else{
if(pattern<248){
if(pattern<244){
if(pattern<242){
if(pattern<241){

        PIXEL00_20
        PIXEL01_60
        PIXEL02_61
        PIXEL03_80
        PIXEL10_60
        PIXEL11_70
        PIXEL12_30
        PIXEL13_10
        PIXEL20_82
        PIXEL21_32
        if (Diff(yy[6], yy[8])) {
                PIXEL22_0
                PIXEL23_0
                PIXEL30_82
                PIXEL31_32
                PIXEL32_0
                PIXEL33_0
        } else {
                PIXEL22_70
                PIXEL23_21
                PIXEL30_11
                PIXEL31_13
                PIXEL32_83
                PIXEL33_50
        }
        

}else{

        PIXEL00_20
        PIXEL01_60
        PIXEL02_61
        PIXEL03_80
        PIXEL10_60
        PIXEL11_70
        PIXEL12_30
        PIXEL13_10
        PIXEL20_82
        PIXEL21_32
        if (Diff(yy[6], yy[8])) {
                PIXEL22_0
                PIXEL23_0
                PIXEL30_82
                PIXEL31_32
                PIXEL32_0
                PIXEL33_0
        } else {
                PIXEL22_70
                PIXEL23_21
                PIXEL30_11
                PIXEL31_13
                PIXEL32_83
                PIXEL33_50
        }
        

}

}else{
if(pattern<243){

        PIXEL00_80
        PIXEL01_10
        if (Diff(yy[2], yy[6])) {
                PIXEL02_10
                PIXEL03_80
                PIXEL12_30
                PIXEL13_10
        } else {
                PIXEL02_11
                PIXEL03_20
                PIXEL12_0
                PIXEL13_12
        }
        PIXEL10_61
        PIXEL11_30
        PIXEL20_82
        PIXEL21_32
        PIXEL22_0
        if (Diff(yy[6], yy[8])) {
                PIXEL23_0
                PIXEL32_0
                PIXEL33_0
        } else {
                PIXEL23_50
                PIXEL32_50
                PIXEL33_50
        }
        PIXEL30_82
        PIXEL31_32
        

}else{

        PIXEL00_81
        PIXEL01_31
        PIXEL02_10
        PIXEL03_80
        PIXEL10_81
        PIXEL11_31
        PIXEL12_30
        PIXEL13_10
        PIXEL20_82
        PIXEL21_32
        if (Diff(yy[6], yy[8])) {
                PIXEL22_0
                PIXEL23_0
                PIXEL30_82
                PIXEL31_32
                PIXEL32_0
                PIXEL33_0
        } else {
                PIXEL22_70
                PIXEL23_21
                PIXEL30_11
                PIXEL31_13
                PIXEL32_83
                PIXEL33_50
        }
        

}

}

}else{
if(pattern<246){
if(pattern<245){

        PIXEL00_20
        PIXEL01_60
        PIXEL02_81
        PIXEL03_81
        PIXEL10_60
        PIXEL11_70
        PIXEL12_31
        PIXEL13_31
        PIXEL20_82
        PIXEL21_32
        PIXEL22_0
        PIXEL23_0
        PIXEL30_82
        PIXEL31_32
        PIXEL32_0
        if (Diff(yy[6], yy[8])) {
                PIXEL33_0
        } else {
                PIXEL33_20
        }
        

}else{

        PIXEL00_20
        PIXEL01_60
        PIXEL02_81
        PIXEL03_81
        PIXEL10_60
        PIXEL11_70
        PIXEL12_31
        PIXEL13_31
        PIXEL20_82
        PIXEL21_32
        PIXEL22_0
        PIXEL23_0
        PIXEL30_82
        PIXEL31_32
        PIXEL32_0
        if (Diff(yy[6], yy[8])) {
                PIXEL33_0
        } else {
                PIXEL33_20
        }
        

}

}else{
if(pattern<247){

        PIXEL00_80
        PIXEL01_10
        if (Diff(yy[2], yy[6])) {
                PIXEL02_0
                PIXEL03_0
                PIXEL13_0
        } else {
                PIXEL02_50
                PIXEL03_50
                PIXEL13_50
        }
        PIXEL10_61
        PIXEL11_30
        PIXEL12_0
        PIXEL20_82
        PIXEL21_32
        PIXEL22_0
        PIXEL23_0
        PIXEL30_82
        PIXEL31_32
        PIXEL32_0
        if (Diff(yy[6], yy[8])) {
                PIXEL33_0
        } else {
                PIXEL33_20
        }
        

}else{

        PIXEL00_81
        PIXEL01_31
        PIXEL02_0
        if (Diff(yy[2], yy[6])) {
                PIXEL03_0
        } else {
                PIXEL03_20
        }
        PIXEL10_81
        PIXEL11_31
        PIXEL12_0
        PIXEL13_0
        PIXEL20_82
        PIXEL21_32
        PIXEL22_0
        PIXEL23_0
        PIXEL30_82
        PIXEL31_32
        PIXEL32_0
        if (Diff(yy[6], yy[8])) {
                PIXEL33_0
        } else {
                PIXEL33_20
        }
        

}

}

}

}else{
if(pattern<252){
if(pattern<250){
if(pattern<249){

        PIXEL00_80
        PIXEL01_61
        PIXEL02_61
        PIXEL03_80
        PIXEL10_10
        PIXEL11_30
        PIXEL12_30
        PIXEL13_10
        if (Diff(yy[8], yy[4])) {
                PIXEL20_0
                PIXEL30_0
                PIXEL31_0
        } else {
                PIXEL20_50
                PIXEL30_50
                PIXEL31_50
        }
        PIXEL21_0
        PIXEL22_0
        if (Diff(yy[6], yy[8])) {
                PIXEL23_0
                PIXEL32_0
                PIXEL33_0
        } else {
                PIXEL23_50
                PIXEL32_50
                PIXEL33_50
        }
        

}else{

        PIXEL00_82
        PIXEL01_82
        PIXEL02_61
        PIXEL03_80
        PIXEL10_32
        PIXEL11_32
        PIXEL12_30
        PIXEL13_10
        PIXEL20_0
        PIXEL21_0
        PIXEL22_0
        if (Diff(yy[6], yy[8])) {
                PIXEL23_0
                PIXEL32_0
                PIXEL33_0
        } else {
                PIXEL23_50
                PIXEL32_50
                PIXEL33_50
        }
        if (Diff(yy[8], yy[4])) {
                PIXEL30_0
        } else {
                PIXEL30_20
        }
        PIXEL31_0
        

}

}else{
if(pattern<251){

        PIXEL00_80
        PIXEL01_10
        PIXEL02_10
        PIXEL03_80
        PIXEL10_10
        PIXEL11_30
        PIXEL12_30
        PIXEL13_10
        if (Diff(yy[8], yy[4])) {
                PIXEL20_0
                PIXEL30_0
                PIXEL31_0
        } else {
                PIXEL20_50
                PIXEL30_50
                PIXEL31_50
        }
        PIXEL21_0
        PIXEL22_0
        if (Diff(yy[6], yy[8])) {
                PIXEL23_0
                PIXEL32_0
                PIXEL33_0
        } else {
                PIXEL23_50
                PIXEL32_50
                PIXEL33_50
        }
        

}else{

        if (Diff(yy[4], yy[2])) {
                PIXEL00_0
                PIXEL01_0
                PIXEL10_0
        } else {
                PIXEL00_50
                PIXEL01_50
                PIXEL10_50
        }
        PIXEL02_10
        PIXEL03_80
        PIXEL11_0
        PIXEL12_30
        PIXEL13_10
        PIXEL20_0
        PIXEL21_0
        PIXEL22_0
        if (Diff(yy[6], yy[8])) {
                PIXEL23_0
                PIXEL32_0
                PIXEL33_0
        } else {
                PIXEL23_50
                PIXEL32_50
                PIXEL33_50
        }
        if (Diff(yy[8], yy[4])) {
                PIXEL30_0
        } else {
                PIXEL30_20
        }
        PIXEL31_0
        

}

}

}else{
if(pattern<254){
if(pattern<253){

        PIXEL00_80
        PIXEL01_61
        PIXEL02_81
        PIXEL03_81
        PIXEL10_10
        PIXEL11_30
        PIXEL12_31
        PIXEL13_31
        if (Diff(yy[8], yy[4])) {
                PIXEL20_0
                PIXEL30_0
                PIXEL31_0
        } else {
                PIXEL20_50
                PIXEL30_50
                PIXEL31_50
        }
        PIXEL21_0
        PIXEL22_0
        PIXEL23_0
        PIXEL32_0
        if (Diff(yy[6], yy[8])) {
                PIXEL33_0
        } else {
                PIXEL33_20
        }
        

}else{

        PIXEL00_82
        PIXEL01_82
        PIXEL02_81
        PIXEL03_81
        PIXEL10_32
        PIXEL11_32
        PIXEL12_31
        PIXEL13_31
        PIXEL20_0
        PIXEL21_0
        PIXEL22_0
        PIXEL23_0
        if (Diff(yy[8], yy[4])) {
                PIXEL30_0
        } else {
                PIXEL30_20
        }
        PIXEL31_0
        PIXEL32_0
        if (Diff(yy[6], yy[8])) {
                PIXEL33_0
        } else {
                PIXEL33_20
        }
        

}

}else{
if(pattern<255){

        PIXEL00_80
        PIXEL01_10
        if (Diff(yy[2], yy[6])) {
                PIXEL02_0
                PIXEL03_0
                PIXEL13_0
        } else {
                PIXEL02_50
                PIXEL03_50
                PIXEL13_50
        }
        PIXEL10_10
        PIXEL11_30
        PIXEL12_0
        if (Diff(yy[8], yy[4])) {
                PIXEL20_0
                PIXEL30_0
                PIXEL31_0
        } else {
                PIXEL20_50
                PIXEL30_50
                PIXEL31_50
        }
        PIXEL21_0
        PIXEL22_0
        PIXEL23_0
        PIXEL32_0
        if (Diff(yy[6], yy[8])) {
                PIXEL33_0
        } else {
                PIXEL33_20
        }
        

}else{

        if (Diff(yy[4], yy[2])) {
                PIXEL00_0
        } else {
                PIXEL00_20
        }
        PIXEL01_0
        PIXEL02_0
        if (Diff(yy[2], yy[6])) {
                PIXEL03_0
        } else {
                PIXEL03_20
        }
        PIXEL10_0
        PIXEL11_0
        PIXEL12_0
        PIXEL13_0
        PIXEL20_0
        PIXEL21_0
        PIXEL22_0
        PIXEL23_0
        if (Diff(yy[8], yy[4])) {
                PIXEL30_0
        } else {
                PIXEL30_20
        }
        PIXEL31_0
        PIXEL32_0
        if (Diff(yy[6], yy[8])) {
                PIXEL33_0
        } else {
                PIXEL33_20
        }
        

}

}

}

}

}

}

}

}
return c[5];
}
void main(){
 ivec3 c[10]; int yy[10]; for(int j=0;j<10;j++){c[j]=ivec3(0);yy[j]=0;}
 vec2 uv=(floor(texCoord*texSize)+0.5)/texSize;
 c[1]=ivec3(floor(texture2D(tex,uv+vec2(-1.0,-1.0)/texSize).rgb*255.0+0.5)); yy[1]=yuv(c[1]);
 c[2]=ivec3(floor(texture2D(tex,uv+vec2(0.0,-1.0)/texSize).rgb*255.0+0.5)); yy[2]=yuv(c[2]);
 c[3]=ivec3(floor(texture2D(tex,uv+vec2(1.0,-1.0)/texSize).rgb*255.0+0.5)); yy[3]=yuv(c[3]);
 c[4]=ivec3(floor(texture2D(tex,uv+vec2(-1.0,0.0)/texSize).rgb*255.0+0.5)); yy[4]=yuv(c[4]);
 c[5]=ivec3(floor(texture2D(tex,uv+vec2(0.0,0.0)/texSize).rgb*255.0+0.5)); yy[5]=yuv(c[5]);
 c[6]=ivec3(floor(texture2D(tex,uv+vec2(1.0,0.0)/texSize).rgb*255.0+0.5)); yy[6]=yuv(c[6]);
 c[7]=ivec3(floor(texture2D(tex,uv+vec2(-1.0,1.0)/texSize).rgb*255.0+0.5)); yy[7]=yuv(c[7]);
 c[8]=ivec3(floor(texture2D(tex,uv+vec2(0.0,1.0)/texSize).rgb*255.0+0.5)); yy[8]=yuv(c[8]);
 c[9]=ivec3(floor(texture2D(tex,uv+vec2(1.0,1.0)/texSize).rgb*255.0+0.5)); yy[9]=yuv(c[9]);
int pattern=0;
if(Diff(yy[5],yy[1])) pattern|=1;
if(Diff(yy[5],yy[2])) pattern|=2;
if(Diff(yy[5],yy[3])) pattern|=4;
if(Diff(yy[5],yy[4])) pattern|=8;
if(Diff(yy[5],yy[6])) pattern|=16;
if(Diff(yy[5],yy[7])) pattern|=32;
if(Diff(yy[5],yy[8])) pattern|=64;
if(Diff(yy[5],yy[9])) pattern|=128;
ivec2 sub=ivec2(floor(fract(texCoord*texSize)*4.0)); int pixel=sub.y*4+sub.x; ivec3 result=c[5];
gl_FragColor=vec4(vec3(calculate(c,yy,pixel,pattern))/255.0,1.0);
}
