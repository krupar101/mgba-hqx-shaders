PYTHON ?= python3
CXX ?= c++
CXXFLAGS ?= -O2
VENDOR := vendor/vbam

.PHONY: generate generated-check repo-check reference classic-reference classic-test classic-native-test classic-4x-test test verify package package-test vba18-test preview clean

generate:
	$(PYTHON) tools/generate.py
	$(PYTHON) tools/generate_classic.py

generated-check:
	$(PYTHON) tests/check_generated.py

repo-check:
	$(PYTHON) tests/check_repo.py
	$(PYTHON) tests/check_docs.py

reference: generate
	$(PYTHON) tests/check_vba18_harness.py
	$(CXX) $(CXXFLAGS) -std=c++11 -shared -fPIC -I$(VENDOR) build/ref2.cpp -o build/ref2.so
	$(CXX) $(CXXFLAGS) -std=c++11 -shared -fPIC -I$(VENDOR) build/ref4.cpp -o build/ref4.so

classic-reference: generate
	$(PYTHON) tests/check_vba18_harness.py
	$(CXX) $(CXXFLAGS) -std=c++11 -shared -fPIC -I$(VENDOR) build/ref2-classic-565.cpp -o build/ref2-classic-565.so
	$(CXX) $(CXXFLAGS) -std=c++11 -shared -fPIC -I$(VENDOR) build/ref2-classic-555.cpp -o build/ref2-classic-555.so

classic-test: classic-reference
	HQX_VARIANT=classic-565 $(PYTHON) -u tests/compare.py 2
	HQX_VARIANT=classic-555 $(PYTHON) -u tests/compare.py 2
	$(PYTHON) tests/check_classic_4x.py

classic-native-test: classic-reference
	HQX_VARIANT=classic-565 $(PYTHON) -u tests/native_frame.py 2 240 160
	HQX_VARIANT=classic-555 $(PYTHON) -u tests/native_frame.py 2 240 160

classic-4x-test: classic-reference
	HQX_VARIANT=classic-565 HQX_SHADER_BUNDLE=hq2x-classic-565-4x.shader HQX_FRAME_LIMIT=16 $(PYTHON) -u tests/compare.py 4
	HQX_VARIANT=classic-555 HQX_SHADER_BUNDLE=hq2x-classic-555-4x.shader HQX_FRAME_LIMIT=16 $(PYTHON) -u tests/compare.py 4
	HQX_VARIANT=classic-565 HQX_SHADER_BUNDLE=hq2x-classic-565-4x.shader $(PYTHON) -u tests/native_frame.py 4 240 160
	HQX_VARIANT=classic-555 HQX_SHADER_BUNDLE=hq2x-classic-555-4x.shader $(PYTHON) -u tests/native_frame.py 4 240 160

test: repo-check reference classic-test
	$(PYTHON) -u tests/compare.py 2
	$(PYTHON) -u tests/compare.py 4
	$(PYTHON) -u tests/native_frame.py 2 240 160
	$(PYTHON) -u tests/native_frame.py 4 240 160

verify:
	$(MAKE) generated-check
	$(MAKE) test

package: generated-check repo-check
	$(PYTHON) tools/package.py

package-test: package
	$(PYTHON) tests/check_package.py

# Supply a directory containing unmodified VBA 1.8 src/hq2x.h and src/interp.h.
vba18-test: generate
	$(PYTHON) -u tests/compare_vba18.py "$(VBA18_SOURCE_DIR)"

preview: reference classic-reference
	$(PYTHON) tools/preview.py
	$(PYTHON) tests/check_preview.py

clean:
	rm -rf build dist
